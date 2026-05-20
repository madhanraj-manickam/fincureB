using FinCure.Repositories;
using FinCure.Repositories.Interfaces;

namespace FinCure.Services
{
    public class SavingsService
    {
        private readonly IincomeRepository _incomeRepository;
        private readonly IExpenseRepository _expenseRepository;

        public SavingsService(
            IincomeRepository incomeRepository,
            IExpenseRepository expenseRepository)
        {
            _incomeRepository = incomeRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<object> GetSavingsSummaryAsync(int userId)
        {
            decimal totalIncome =
                await _incomeRepository.GetTotalIncomeAsync(userId);

            decimal totalExpense =
                await _expenseRepository.GetTotalExpenseAsync(userId);

            decimal totalSavings = totalIncome - totalExpense;

            decimal savingsRate = 0;

            if (totalIncome > 0)
            {
                savingsRate = (totalSavings / totalIncome) * 100;
            }

            return new
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                TotalSavings = totalSavings,
                SavingsRate = Math.Round(savingsRate, 2)
            };
        }
    }
}