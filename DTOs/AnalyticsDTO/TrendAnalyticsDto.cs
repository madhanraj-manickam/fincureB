namespace FinCure.DTOs.AnalyticsDTO
{
    public class TrendAnalyticsDto
    {
        public string PeriodLabel { get; set; } = string.Empty; 
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}