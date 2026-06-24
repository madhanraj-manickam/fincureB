using ExpenseTracker.API.DTOs.Expense;

using FinCure.Models;
using FinCure.Repositories.Interfaces;


namespace FinCure.Services
{
    public class ExpenseService 
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<ExpenseResponseDto> AddExpenseAsync(int userId, ExpenseCreateDto dto)
        {
            var expense = new Expense
            {
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,
                Date = dto.Date.Date,
                UserId = userId
            };

            await _expenseRepository.AddAsync(expense);

            return MapToDto(expense);
        }

        public async Task<List<ExpenseResponseDto>> GetAllAsync(int userId)
        {
            var expenses = await _expenseRepository.GetAllByUserIdAsync(userId);
            return expenses.Select(MapToDto).ToList();
        }

        public async Task<ExpenseResponseDto?> GetByIdAsync(int id, int userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id, userId);

            if (expense == null)
                return null;

            return MapToDto(expense);
        }

        public async Task<List<ExpenseResponseDto>> GetByCategoryAsync(int userId, string category)
        {
            var expenses = await _expenseRepository.GetByCategoryAsync(userId, category);
            return expenses.Select(MapToDto).ToList();
        }

        public async Task<ExpenseResponseDto?> UpdateAsync(int id, int userId, ExpenseUpdateDto dto)
        {
            var expense = await _expenseRepository.GetByIdAsync(id, userId);

            if (expense == null)
                return null;

            expense.Amount = dto.Amount;
            expense.Category = dto.Category;
            expense.Description = dto.Description;

            await _expenseRepository.UpdateAsync(expense);

            return MapToDto(expense);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var expense = await _expenseRepository.GetByIdAsync(id, userId);

            if (expense == null)
                return false;

            await _expenseRepository.DeleteAsync(expense);

            return true;
        }

        private ExpenseResponseDto MapToDto(Expense expense)
        {
            return new ExpenseResponseDto
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Description = expense.Description,
                Date = expense.Date
            };
        }

        // ExpenseService.cs
        public async Task<decimal> GetTotalExpenseAsync(int userId)
        {
            return await _expenseRepository.GetTotalExpenseAsync(userId);
        }


    }
}