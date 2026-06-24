using FinCure.DTOs.AuthDTO;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
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

        public async Task<Users> RegisterAsync(RegisterDto registerDto)
        {
            // 1. Failsafe Null Checks
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto));

            if (string.IsNullOrWhiteSpace(registerDto.Email))
                throw new ArgumentException("Email address is required.");
            //PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            if (string.IsNullOrWhiteSpace(registerDto.Password))
                throw new ArgumentException("Password is required.");

            // 2. Duplicate Email Validation (CRITICAL)
            // We check the DB to see if anyone else has this email
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                // Throwing this exception will be caught by your ExceptionMiddleware
                // and sent to Angular as a  error message.
                throw new InvalidOperationException("An account with this email address already exists.");
            }

            var user = new Users
            {
                UserName = registerDto.UserName.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email.ToLower().Trim(),
                PhoneNumber=registerDto.PhoneNumber

            };



            // 3. Hash Password and Save
            
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
            if (!BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash))
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

       
    