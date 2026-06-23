using FinCure.Configurations;
using FinCure.Data;
using FinCure.Repositories;
using FinCure.Repositories.Interfaces;
using FinCure.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinCure.Middleware;

using DotNetEnv;



var builder = WebApplication.CreateBuilder(args);

Env.Load();

var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();



builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularUI",
        policy => policy.WithOrigins("http://localhost:4200") // Your Angular URL
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<FinCureDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["AppSettings:Token"]!
                )
            ),
            ValidateIssuer = true,
            ValidIssuer = "FinCure",
            ValidateAudience = true,
            ValidAudience = "Users",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IincomeRepository, IncomeRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<InvestmentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IncomeService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<SavingsService>();
builder.Services.AddScoped<RecommendationService>();

//builder.Configuration["GoogleSettings:ApiKey"];
//Gemini API settings
builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));


builder.Services.AddScoped<GeminiService>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        "recommendation-policy",
        config =>
        {
            config.PermitLimit = 5;
            config.Window = TimeSpan.FromMinutes(1);
        });
});





var app = builder.Build();
app.UseCors("AllowAngularUI");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseRateLimiter();

app.Run();