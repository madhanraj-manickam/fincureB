using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense> AddAsync(Expense expense);
        Task<List<Expense>> GetAllByUserIdAsync(int userId);
        Task<Expense?> GetByIdAsync(int id, int userId);
        Task<List<Expense>> GetByCategoryAsync(int userId, string category);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
        Task<decimal> GetTotalExpenseAsync(int userId);
    }
}