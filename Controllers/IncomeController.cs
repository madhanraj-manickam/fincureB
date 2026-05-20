using FinCure.DTOs.Income;
using FinCure.DTOs.IncomeDTO;
using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IncomeService _incomeService;

        public IncomeController(IncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddIncome(IncomeCreateDto dto)
        {
            int userId = GetUserId();
            var result = await _incomeService.AddIncomeAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncome()
        {
            int userId = GetUserId();
            var result = await _incomeService.GetAllAsync(userId);
            return Ok(result);
        }
        [HttpGet("category/{category}")]
public async Task<IActionResult> GetByCategory(string category)
{
    int userId = GetUserId();
    var result = await _incomeService.GetByCategoryAsync(userId, category);
    return Ok(result);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateIncome(int id, IncomeUpdateDto dto)
{
    int userId = GetUserId();
    var result = await _incomeService.UpdateAsync(id, userId, dto);

    if (result == null)
        return NotFound();

    return Ok(result);
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteIncome(int id)
{
    int userId = GetUserId();
    bool deleted = await _incomeService.DeleteAsync(id, userId);

    if (!deleted)
        return NotFound();

    return Ok(new { message = "Income deleted successfully" });
}

private int GetUserId()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userIdClaim == null)
        throw new UnauthorizedAccessException("User not found.");

    return int.Parse(userIdClaim);
}
    }
}