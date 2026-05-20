using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class IncomeRepository : IincomeRepository
    {
        private readonly FinCureDBContext _context;

        public IncomeRepository(FinCureDBContext context)
{
    _context = context;
}

public async Task<Income> AddAsync(Income income)
{
    await _context.Incomes.AddAsync(income);
    await _context.SaveChangesAsync();
    return income;
}

public async Task<List<Income>> GetAllByUserIdAsync(int userId)
{
    return await _context.Incomes
        .Where(i => i.UserId == userId)
        .OrderByDescending(i => i.Date)
        .ToListAsync();
}

public async Task<Income?> GetByIdAsync(int id, int userId)
{
    return await _context.Incomes
        .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
}

public async Task<List<Income>> GetByCategoryAsync(int userId, string category)
{
    return await _context.Incomes
        .Where(i => i.UserId == userId &&
                    i.Category.ToLower() == category.ToLower())
        .OrderByDescending(i => i.Date)
        .ToListAsync();
}

public async Task UpdateAsync(Income income)
{
    _context.Incomes.Update(income);
    await _context.SaveChangesAsync();
}

public async Task DeleteAsync(Income income)
{
    _context.Incomes.Remove(income);
    await _context.SaveChangesAsync();
}

        public async Task<decimal> GetTotalIncomeAsync(int userId)
        {
            return await _context.Incomes
                .Where(i => i.UserId == userId)
                .SumAsync(i => (decimal?)i.Amount) ?? 0;
        }

    }
}