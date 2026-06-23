using FinCure.Data;
using FinCure.DTOs.AuthDTO;
using FinCure.Models;
using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FinCure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FinCureDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly AuthService _authService;

        public AuthController(FinCureDBContext context, IConfiguration configuration, IMemoryCache cache,AuthService authService )
        {
            _context = context;
            _configuration = configuration;
            _cache = cache;
            _authService = authService;
        }

        // Register a new User
        [HttpPost("register")]
        public async Task<ActionResult<String>> Register(RegisterDto dto)
        {
            var user = new Users
            {
                UserName = dto.UserName,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return Ok("User Registered Successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LogInDTO dto)
        {
            try
            {
                // Let the service handle the business logic and token generation
                var result = await _authService.LoginAsync(dto);

                // Return the successful response formatted exactly how Angular expects it
                return Ok(new
                {
                    token = result.Token,
                    user = new
                    {
                        id = result.User.UserId,
                        username = result.User.UserName,
                        email = result.User.Email,
                        phoneNumber = result.User.PhoneNumber
                    }
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // If the service throws an error (e.g., "Wrong Password"), catch it and return a 400
                return BadRequest(ex.Message);
            }
        }
    

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid request. Missing or malformed token.");
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            // Since your token lifetime is explicitly set to 1 Hour in CreateToken(),
            // cache it for 1 hour. Once 1 hour passes, the token naturally expires anyway,
            // so it will automatically clear itself from memory cache storage.
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };

            // Add the token to the server blacklist
            _cache.Set(token, true, cacheOptions);

            return Ok(new { message = "Logged out successfully. Token has been permanently invalidated on the server." });
        }
        private string CreateToken(Users user)
        {
            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),

    new Claim(ClaimTypes.Name, user.UserName),

    new Claim(ClaimTypes.Email, user.Email)
};



            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value!
                ));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer:"FinCure",
                audience:"Users",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
