using FinCure.DTOs;
using FinCure.Repositories.Interfaces;

namespace FinCure.Services
{
    public class RecommendationService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IincomeRepository _incomeRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly GeminiService _geminiService;

        public RecommendationService(
            IincomeRepository incomeRepository,
            IExpenseRepository expenseRepository,
            IInvestmentRepository investmentRepository,
            GeminiService geminiService)
        {
            _incomeRepository = incomeRepository;
            _expenseRepository = expenseRepository;
            _investmentRepository = investmentRepository;
            _geminiService = geminiService;
        }

        public async Task<RecommendationResponseDto> GetRecommendationsAsync(int userId)
        {
            var incomes =
                await _incomeRepository.GetAllByUserIdAsync(userId);

            var expenses =
                await _expenseRepository.GetAllByUserIdAsync(userId);

            var investments =
                await _investmentRepository.GetAllByUserIdAsync(userId);

            decimal totalIncome =
                incomes.Sum(i => i.Amount);

            decimal totalExpense =
                expenses.Sum(e => e.Amount);

            decimal totalInvestment =
                investments.Sum(i => i.AmountInvested);

            var recommendations = new List<string>();

            int score = 100;

            if (totalIncome > 0)
            {
                decimal expenseRatio =
                    (totalExpense / totalIncome) * 100;

                if (expenseRatio > 80)
                {
                    recommendations.Add(
                        "You are spending more than 80% of your income.");

                    score -= 20;
                }

                if (expenseRatio > 100)
                {
                    recommendations.Add(
                        "Your expenses exceed your income.");

                    score -= 20;
                }

                decimal savings =
                    totalIncome - totalExpense;

                decimal savingsRate =
                    (savings / totalIncome) * 100;

                if (savingsRate < 10)
                {
                    recommendations.Add(
                        "Your savings rate is below 10%.");

                    score -= 20;
                }

                if (savingsRate > 30)
                {
                    recommendations.Add(
                        "Excellent savings habit. Keep it up.");
                }

                if (totalInvestment == 0)
                {
                    recommendations.Add(
                        "Consider investing at least 10% of your income.");

                    score -= 15;
                }
                else if ((totalInvestment / totalIncome) * 100 < 10)
                {
                    recommendations.Add(
                        "Your investment amount is below the recommended level.");

                    score -= 10;
                }
            }

            var foodExpense =
                expenses
                .Where(e => e.Category == "Food")
                .Sum(e => e.Amount);

            if (totalExpense > 0)
            {
                decimal foodPercentage =
                    (foodExpense / totalExpense) * 100;

                if (foodPercentage > 30)
                {
                    recommendations.Add(
                        "Food expenses account for more than 30% of your spending.");

                    score -= 10;
                }
            }

            if (score < 0)
            {
                score = 0;
            }
            var config = new 
            {
               
                TopK = 40,          // chosse the 40 most relevent outcomes
                TopP = 0.95f        // Limits the pool to words whose cumulative probability equals 95%
            };

            string prompt = $@"
You are a professional financial advisor.

Financial Summary:

Total Income: {totalIncome}

Total Expense: {totalExpense}

Total Investment: {totalInvestment}

Financial Health Score: {score}

System Generated Findings:
{string.Join("\n", recommendations)}

Based on the above analysis:

1. Explain the user's financial situation.
2. Suggest spending improvements if necessary.
3. Suggest investment improvements if necessary.
4. Keep the response under 150 words.
5. Use simple language.
";

            string aiRecommendation =
                await _geminiService.GenerateRecommendationAsync(prompt,config);

            return new RecommendationResponseDto
            {
                FinancialHealthScore = score,
                Recommendations = recommendations,
                AiRecommendation = aiRecommendation
            };
        }
    }
}