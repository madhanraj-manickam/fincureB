namespace FinCure.DTOs.AuthDTO
{
    public class RegisterDto
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public String? Email { get; set; }

        public long PhoneNumber { get; set; }
    }
}