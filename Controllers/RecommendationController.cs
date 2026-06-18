using FinCure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinCure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;

        public RecommendationController(
            RecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            int userId = GetUserId();

            var result =
                await _recommendationService
                    .GetRecommendationsAsync(userId);

            return Ok(result);
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