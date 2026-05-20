using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SavingsController : ControllerBase
    {
        private readonly SavingsService _savingsService;

        public SavingsController(SavingsService savingsService)
        {
            _savingsService = savingsService;
        }

        [HttpGet("savings")]
        public async Task<IActionResult> GetSavingsSummary()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User not found.");
            }

            int userId = int.Parse(userIdClaim);

            var result =
                await _savingsService.GetSavingsSummaryAsync(userId);

            return Ok(result);
        }
    }
}