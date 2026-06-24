using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinCure.Models
{
    public class Accounts
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string AccountType { get; set; } = "Bank"; // Bank, Credit, Cash, MobileWallet

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        [ForeignKey("UserId")]
        public virtual Users? User { get; set; }
    }
}