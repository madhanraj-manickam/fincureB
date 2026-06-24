using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly FinCureDBContext _context;

        public AnalyticsRepository(FinCureDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int userId, string type, DateTime startDate, DateTime endDate)
        {
            DateTime startBoundary = startDate.Date;
            DateTime nextDayBoundary = endDate.Date.AddDays(1);

            if (type == "Income")
            {
                return await _context.Incomes
                    .Where(i => i.UserId == userId && i.Date >= startBoundary && i.Date < nextDayBoundary)
                    .Select(i => new Transaction { Category = i.Category, Amount = i.Amount, Type = "Income", TransactionDate = i.Date })
                    .ToListAsync();
            }
            else
            {
                return await _context.Expenses
                    .Where(e => e.UserId == userId && e.Date >= startBoundary && e.Date < nextDayBoundary)
                    .Select(e => new Transaction { Category = e.Category, Amount = e.Amount, Type = "Expense", TransactionDate = e.Date })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsInRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            DateTime startBoundary = startDate.Date;
            DateTime nextDayBoundary = endDate.Date.AddDays(1);

            var incomes = await _context.Incomes
                .Where(i => i.UserId == userId && i.Date >= startBoundary && i.Date < nextDayBoundary)
                .Select(i => new Transaction { Amount = i.Amount, Type = "Income", TransactionDate = i.Date })
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date >= startBoundary && e.Date < nextDayBoundary)
                .Select(e => new Transaction { Amount = e.Amount, Type = "Expense", TransactionDate = e.Date })
                .ToListAsync();

            return incomes.Concat(expenses).OrderBy(t => t.TransactionDate);
        }
    }
}