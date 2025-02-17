using HumanResourceManagementSystem.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HumanResourceManagementSystem.Api.Middlewares
{
    public class TokenBlacklistValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenBlacklistValidationMiddleware> _logger;

        public TokenBlacklistValidationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TokenBlacklistValidationMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tokenService = context.RequestServices.GetRequiredService<ITokenService>();

            var accessToken = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var isBlacklisted = await tokenService.IsAccessTokenBlacklistedAsync(accessToken);
                if (isBlacklisted)
                {
                    _logger.LogWarning("Token validation failed: Token is blacklisted");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Token has been blacklisted" });
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class TokenBlacklistValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenBlacklistValidation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenBlacklistValidationMiddleware>();
        }
    }
}

