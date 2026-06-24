using FinCure.Data;
using FinCure.Models;
using FinCure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinCure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinCureDBContext _context;

        public TransactionRepository(FinCureDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Clean boundaries without fractions of a second
            DateTime startBoundary = startDate.Date;
            DateTime nextDayBoundary = endDate.Date.AddDays(1); // Exactly 00:00:00 of the following day

            var incomes = await _context.Incomes
           
                .Where(i => i.UserId == userId && i.Date >= startBoundary && i.Date < nextDayBoundary)
                .Select(i => new Transaction
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    Amount = i.Amount,
                    Type = "Income",
                    Category = i.Category,
                    Title = "Income Added",
                   
                    TransactionDate = i.Date
                }).ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date >= startBoundary && e.Date < nextDayBoundary)
                .Select(e => new Transaction
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    Amount = e.Amount,
                    Type = "Expense",
                    Category = e.Category,
                   
                    Description = e.Description,
                    TransactionDate = e.Date
                }).ToListAsync();

            return incomes.Concat(expenses).OrderByDescending(t => t.TransactionDate);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByMonthAsync(int userId, int year, int month)
        {
            var incomes = await _context.Incomes
                .Where(i => i.UserId == userId && i.Date.Year == year && i.Date.Month == month)
                .Select(i => new Transaction { Amount = i.Amount, Type = "Income", TransactionDate = i.Date })
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date.Year == year && e.Date.Month == month)
                .Select(e => new Transaction { Amount = e.Amount, Type = "Expense", TransactionDate = e.Date })
                .ToListAsync();

            return incomes.Concat(expenses);
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction) => throw new NotImplementedException();
        public async Task<Transaction?> GetTransactionByIdAsync(int id) => throw new NotImplementedException();
        public async Task DeleteTransactionAsync(Transaction transaction) => throw new NotImplementedException();
    }
}