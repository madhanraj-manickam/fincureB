namespace FinCure.DTOs.InvestmentDTO
{
    public class UpdateInvestmentDto
    {
        public string? InvestmentName { get; set; }

        public string? InvestmentType { get; set; }

        public decimal AmountInvested { get; set; }

        public DateTime StartDate { get; set; }

        public string? Notes { get; set; }
    }
}