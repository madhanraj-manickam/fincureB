namespace FinCure.DTOs.InvestmentDTO
{
    public class PortfolioSummaryDto
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal TotalSavings { get; set; }

        public decimal TotalInvested { get; set; }

        public decimal RemainingSavings { get; set; }

        public int TotalInvestments { get; set; }
    }
}