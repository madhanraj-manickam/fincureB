using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly FinCureDBContext _context;

        public ExpenseRepository(FinCureDBContext context)
        {
            _context = context;
        }

        public async Task<Expense> AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<List<Expense>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(int id, int userId)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task<List<Expense>> GetByCategoryAsync(int userId, string category)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.Category.ToLower() == category.ToLower())
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalExpenseAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;
        }
    }
}