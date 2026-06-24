using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string period = "daily")
        {
            int userId = GetUserId();

          //  DateTime endOfDay = endDate.Date.AddDays(1).AddTicks(-1);

            var result = await _transactionService.GetPeriodSummaryAsync(userId, startDate, endDate, period);
            return Ok(result);
        }

        [HttpGet("calendar")]
        public async Task<IActionResult> GetCalendar([FromQuery] int year, [FromQuery] int month)
        {
            int userId = GetUserId();
            var result = await _transactionService.GetCalendarDataAsync(userId, year, month);
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