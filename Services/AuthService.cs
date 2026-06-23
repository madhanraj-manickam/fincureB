using FinCure.DTOs.AuthDTO;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinCure.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<Users> RegisterAsync(Users user)
        {
            // 1. Failsafe Null Checks
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email address is required.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("Password is required.");

            // 2. Duplicate Email Validation (CRITICAL)
            // We check the DB to see if anyone else has this email
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                // Throwing this exception will be caught by your ExceptionMiddleware
                // and sent to Angular as a beautiful error message.
                throw new InvalidOperationException("An account with this email address already exists.");
            }

            // 3. Hash Password and Save
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            return await _userRepository.AddUserAsync(user);
        }

        public async Task<(string Token, Users User)> LoginAsync(LogInDTO dto)
        {
            // The Service asks the Repository for data, knowing nothing about the underlying SQL/Entity Framework
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is Not Available");
            }

            // Note: If you implement password hashing later, update this comparison
            if (user.Password != dto.Password)
            {
                throw new UnauthorizedAccessException("Wrong Password");
            }

            string token = CreateToken(user);
            return (token, user);
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
                issuer: "FinCure",
                audience: "Users",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

       
    