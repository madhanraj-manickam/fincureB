using FinCure.DTOs.InvestmentDTO;
using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvestmentController : ControllerBase
    {
        private readonly InvestmentService _investmentService;

        public InvestmentController(InvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvestment(
            CreateInvestmentDto dto)
        {
            int userId = GetUserId();

            var result =
                await _investmentService.AddInvestmentAsync(
                    userId,
                    dto);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvestments()
        {
            int userId = GetUserId();

            var result =
                await _investmentService.GetAllAsync(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvestmentById(int id)
        {
            int userId = GetUserId();

            var result =
                await _investmentService.GetByIdAsync(
                    id,
                    userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvestment(
            int id,
            UpdateInvestmentDto dto)
        {
            int userId = GetUserId();

            var result =
                await _investmentService.UpdateAsync(
                    id,
                    userId,
                    dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvestment(int id)
        {
            int userId = GetUserId();

            bool deleted =
                await _investmentService.DeleteAsync(
                    id,
                    userId);

            if (!deleted)
                return NotFound();

            return Ok(new
            {
                message = "Investment deleted successfully"
            });
        }

        private int GetUserId()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
                throw new UnauthorizedAccessException(
                    "User not found.");

            return int.Parse(userIdClaim);
        }
    }
}