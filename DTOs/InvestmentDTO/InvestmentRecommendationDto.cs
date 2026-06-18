namespace FinCure.DTOs.InvestmentDTO
{
    public class InvestmentRecommendationDto
    {
        public string? RecommendedInvestmentType { get; set; }

        public string? RiskLevel { get; set; }

        public decimal ExpectedReturnPercentage { get; set; }

        public string? Reason { get; set; }
    }
}