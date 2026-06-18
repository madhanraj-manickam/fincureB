namespace FinCure.DTOs
{
    public class RecommendationResponseDto
    {
        public int FinancialHealthScore { get; set; }
        public List<string> Recommendations { get; set; } = new();
        public string AiRecommendation { get; internal set; }
    }
}
