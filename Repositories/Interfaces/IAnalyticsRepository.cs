using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface IAnalyticsRepository
    {
        // Fetches raw transactions for a specific type (Income or Expense) to group by category
        Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int userId, string type, DateTime startDate, DateTime endDate);

        // Fetches all transactions in a date range to calculate trends
        Task<IEnumerable<Transaction>> GetAllTransactionsInRangeAsync(int userId, DateTime startDate, DateTime endDate);
    }
}