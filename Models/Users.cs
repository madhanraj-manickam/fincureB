using System.ComponentModel.DataAnnotations;

namespace FinCure.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public String? UserName { get; set; }
        public String? Password { get; set; }
        public String? Email { get; set; }
        public long PhoneNumber { get; set; }
        
    }
}
