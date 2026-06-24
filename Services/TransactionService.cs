using FinCure.DTOs.TransactionDTO;
using FinCure.Models;
using FinCure.Repositories.Interfaces;

namespace FinCure.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        // Injecting the Interface ensures loose coupling
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // --- 1. PERIOD AGGREGATION ---
        public async Task<PeriodSummaryDto> GetPeriodSummaryAsync(int userId, DateTime startDate, DateTime endDate, string periodType)
        {
            // Fetch raw data from Repo
            var rawTransactions = await _transactionRepository.GetTransactionsByDateRangeAsync(userId, startDate, endDate);

            var summary = new PeriodSummaryDto
            {
                TotalIncome = rawTransactions.Where(t => t.Type == "Income").Sum(t => t.Amount),
                TotalExpense = rawTransactions.Where(t => t.Type == "Expense").Sum(t => t.Amount),
            };
            summary.NetBalance = summary.TotalIncome - summary.TotalExpense;

            // Group transactions based on the requested view format
            IEnumerable<IGrouping<string, Transaction>> groupedData = periodType.ToLower() switch
            {
                "monthly" => rawTransactions.GroupBy(t => t.TransactionDate.ToString("yyyy-MMM")),
                "yearly" => rawTransactions.GroupBy(t => t.TransactionDate.ToString("yyyy")),
                "daily" => rawTransactions.GroupBy(t => t.TransactionDate.ToString("yyyy-MM-dd")),
                 _=> rawTransactions.GroupBy(t => t.TransactionDate.ToString("yyyy-MM-dd"))
            };

            summary.GroupedTransactions = groupedData.ToDictionary(
                g => g.Key,
                g => g.Select(t => new TransactionResponseDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Category = t.Category,
                    Title = t.Title,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    AccountName = t.Account?.AccountName
                }).ToList()
            );

            return summary;
        }

        // --- 2. CALENDAR AGGREGATION ---
        public async Task<List<CalendarDayDto>> GetCalendarDataAsync(int userId, int year, int month)
        {
            // Fetch raw data from Repo
            var transactions = await _transactionRepository.GetTransactionsByMonthAsync(userId, year, month);

            var calendarData = transactions
                .GroupBy(t => t.TransactionDate.Date)
                .Select(g => new CalendarDayDto
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    TotalIncome = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                    TotalExpense = g.Where(t => t.Type == "Expense").Sum(t => t.Amount),
                    NetBalance = g.Where(t => t.Type == "Income").Sum(t => t.Amount) - g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
                })
                .OrderBy(d => d.Date)
                .ToList();

            return calendarData;
        }
    }
}