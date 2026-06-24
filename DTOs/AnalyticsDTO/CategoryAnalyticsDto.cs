namespace FinCure.DTOs.AnalyticsDTO
{
    public class CategoryAnalyticsDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; } // We will calculate this in the Service
    }
}