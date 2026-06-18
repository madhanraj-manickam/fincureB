using FinCure.Models;

namespace FinCure.Repositories.Interfaces
{
    public interface IInvestmentRepository
    {
        Task<Investment> AddAsync(Investment investment);

        Task<IEnumerable<Investment>> GetAllAsync();

        Task<Investment?> GetByIdAsync(int id);

        Task<IEnumerable<Investment>> GetAllByUserIdAsync(int userId);


        Task UpdateAsync(Investment investment);

        Task DeleteAsync(int id);
    }
}