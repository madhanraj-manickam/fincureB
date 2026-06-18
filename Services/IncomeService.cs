using FinCure.DTOs.Income;
using FinCure.DTOs.IncomeDTO;
using FinCure.Models;
using FinCure.Repositories.Interfaces;




namespace FinCure.Services 
{
    public class IncomeService 
    {
        private readonly IincomeRepository _incomeRepository;

        public IncomeService(IincomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task<IncomeResponseDto> AddIncomeAsync(int userId, IncomeCreateDto dto)
        {
            var income = new Income
            {
                Amount = dto.Amount,                  // can use mapper to overcome manual assigninig
                Category = dto.Category,
               
                Date = DateTime.UtcNow,
                UserId = userId
            };

            await _incomeRepository.AddAsync(income);

            return MapToDto(income);
        }

        public async Task<List<IncomeResponseDto>> GetAllAsync(int userId)
        {
            try
            {
                var incomes = await _incomeRepository.GetAllByUserIdAsync(userId);

                if (incomes == null || !incomes.Any())
                {
                    throw new Exception("No income records found.");
                }

                return incomes.Select(MapToDto).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncomeResponseDto?> GetByIdAsync(int id, int userId)
        {
            var income = await _incomeRepository.GetByIdAsync(id, userId);
            return income == null ? null : MapToDto(income);
        }

        public async Task<List<IncomeResponseDto>> GetByCategoryAsync(int userId, string category)
{
    var incomes = await _incomeRepository.GetByCategoryAsync(userId, category);
    return incomes.Select(MapToDto).ToList();
}

public async Task<IncomeResponseDto?> UpdateAsync(int id, int userId, IncomeUpdateDto dto)
{
    var income = await _incomeRepository.GetByIdAsync(id, userId);
    if (income == null)
        return null;

    income.Amount = dto.Amount;
    income.Category = dto.Category;
    

    await _incomeRepository.UpdateAsync(income);

    return MapToDto(income);
}

public async Task<bool> DeleteAsync(int id, int userId)
{
    var income = await _incomeRepository.GetByIdAsync(id, userId);
    if (income == null)
        return false;

    await _incomeRepository.DeleteAsync(income);
    return true;
}

private static IncomeResponseDto MapToDto(Income income)
{
    return new IncomeResponseDto
    {
        Id = income.Id,
        Amount = income.Amount,
        Category = income.Category,
    
        Date = income.Date
    };
}

        // IncomeService.cs
        public async Task<decimal> GetTotalIncomeAsync(int userId)
        {
            return await _incomeRepository.GetTotalIncomeAsync(userId);
        }


    }
}