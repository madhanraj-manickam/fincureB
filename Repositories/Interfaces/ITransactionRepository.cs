using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        // Core methods for Aggregations
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetTransactionsByMonthAsync(int userId, int year, int month);

        // Standard CRUD methods for later use
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task DeleteTransactionAsync(Transaction transaction);
    }
}