using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly FinCureDBContext _context;

        public InvestmentRepository(FinCureDBContext context)
        {
            _context = context;
        }

        public async Task<Investment> AddAsync(Investment investment)
        {
            await _context.Investments.AddAsync(investment);
            await _context.SaveChangesAsync();

            return investment;
        }

        public async Task<IEnumerable<Investment>> GetAllAsync()
        {
            return await _context.Investments.ToListAsync();
        }

        public async Task<Investment?> GetByIdAsync(int id)
        {
            return await _context.Investments
                .FirstOrDefaultAsync(i => i.UserId == id);
        }

        public async Task<IEnumerable<Investment>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Investments
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Investment investment)
        {
            _context.Investments.Update(investment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var investment = await _context.Investments
                .FirstOrDefaultAsync(i => i.UserId == id);

            if (investment != null)
            {
                _context.Investments.Remove(investment);
                await _context.SaveChangesAsync();
            }
        }
       
    }
}