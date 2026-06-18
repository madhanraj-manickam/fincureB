using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace FinCure.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public TokenBlacklistMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the bearer token from the Authorization request header
            string authHeader = context.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                // If the token matches a key inside our cache blacklist, block access immediately
                if (_cache.TryGetValue(token, out _))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { message = "Unauthorized: This token has been invalidated via logout." });
                    return;
                }
            }

            // Token is clean, proceed to the next layer
            await _next(context);
        }
    }
}