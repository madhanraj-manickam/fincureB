using FinCure.Data;
using FinCure.DTOs.AuthDTO;
using FinCure.Models;
using Microsoft.AspNetCore.Mvc;
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
        public AuthController(FinCureDBContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public async Task<ActionResult<String>> Login(LogInDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == dto.Email);

            if (user == null)
            {
                return BadRequest("User is Not Available");
            }

            if(user.Password != dto.Password)
            {
                return BadRequest("Wrong Password");
            }
            String token = CreateToken(user);
            return Ok(new
            {
                token = token,

                user = new
                {
                    id = user.UserId,
                    username = user.UserName,
                    email = user.Email,
                    phoneNumber = user.PhoneNumber
                }
            });
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
