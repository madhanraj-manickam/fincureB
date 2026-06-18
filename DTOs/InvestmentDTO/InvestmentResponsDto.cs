namespace FinCure.DTOs.InvestmentDTO
{
    public class InvestmentResponseDto
    {
        public int InvestmentId { get; set; }

        public string? InvestmentName { get; set; }

        public string? InvestmentType { get; set; }

        public decimal AmountInvested { get; set; }

        public string? RiskLevel { get; set; }

        public decimal ExpectedReturnPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}