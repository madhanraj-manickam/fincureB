using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface IincomeRepository
    {
        Task<Income> AddAsync(Income income);
        Task<List<Income>> GetAllByUserIdAsync(int userId);
        Task<Income?> GetByIdAsync(int id, int userId);
        Task<List<Income>> GetByCategoryAsync(int userId, string category);
        Task UpdateAsync(Income income);
        Task DeleteAsync(Income income);
        Task<decimal> GetTotalIncomeAsync(int userId);
    }
}