using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryAnalytics([FromQuery] string type, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            int userId = GetUserId();
           // DateTime endOfDay = endDate.Date.AddDays(1).AddTicks(-1);
            var result = await _analyticsService.GetCategoryBreakdownAsync(userId, type, startDate, endDate);
            return Ok(result);
        }

        [HttpGet("trend/monthly")]
        public async Task<IActionResult> GetMonthlyTrend([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            int userId = GetUserId();
           // DateTime endOfDay = endDate.Date.AddDays(1).AddTicks(-1);
            var result = await _analyticsService.GetMonthlyTrendAsync(userId, startDate, endDate);
            return Ok(result);
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) throw new UnauthorizedAccessException("Invalid token.");
            return int.Parse(userIdClaim);
        }
    }
}