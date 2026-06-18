using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinCure.Models
{
    public class Investment
    {
        [Key]
        public int InvestmentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? InvestmentName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? InvestmentType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountInvested { get; set; }

        public DateTime StartDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Users? User { get; set; }
    }
}