namespace FinCure.DTOs.InvestmentDTO
{
    public class InvestmentInsightDto
    {
        public decimal SavingsRate { get; set; }

        public decimal InvestmentRate { get; set; }

        public string? FinancialHealth { get; set; }

        public string? Suggestion { get; set; }
    }
}
