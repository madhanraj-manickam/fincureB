using FinCure.DTOs.AnalyticsDTO;
using FinCure.Repositories.Interfaces;

namespace FinCure.Services
{
    public class AnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsService(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        // --- 1. CATEGORY BREAKDOWN (For Pie/Doughnut Charts) ---
        public async Task<List<CategoryAnalyticsDto>> GetCategoryBreakdownAsync(int userId, string type, DateTime startDate, DateTime endDate)
        {
            var transactions = await _analyticsRepository.GetTransactionsByTypeAsync(userId, type, startDate, endDate);

            var totalAmount = transactions.Sum(t => t.Amount);
            if (totalAmount == 0) return new List<CategoryAnalyticsDto>();

            var breakdown = transactions
                .GroupBy(t => t.Category)
                .Select(g => new CategoryAnalyticsDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount),
                    // Safely calculate the percentage and round to 2 decimal places
                    Percentage = Math.Round((g.Sum(t => t.Amount) / totalAmount) * 100, 2)
                })
                .OrderByDescending(c => c.Amount) // Largest categories first
                .ToList();

            return breakdown;
        }

        // --- 2. MONTHLY SPENDING TREND (For Line/Bar Charts) ---
        public async Task<List<TrendAnalyticsDto>> GetMonthlyTrendAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var transactions = await _analyticsRepository.GetAllTransactionsInRangeAsync(userId, startDate, endDate);

            var trend = transactions
                // Grouping safely using anonymous objects for EF Core/LINQ compatibility
                .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
                .Select(g => new TrendAnalyticsDto
                {
                    // Formats to "Jan 2026", "Feb 2026", etc.
                    PeriodLabel = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    Income = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    Expense = g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .ToList();

            return trend;
        }
    }
}