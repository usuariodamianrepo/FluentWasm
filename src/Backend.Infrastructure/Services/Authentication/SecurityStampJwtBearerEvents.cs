namespace Backend.Infrastructure.Services.Authentication
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    using Backend.Domain.Entities.Identity;
    using Backend.Infrastructure.Configuration;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public sealed class SecurityStampJwtBearerEvents : JwtBearerEvents
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SecurityStampValidationOptions _options;
        private readonly ILogger<SecurityStampJwtBearerEvents> _logger;

        public SecurityStampJwtBearerEvents(
            UserManager<AppUser> userManager,
            IOptions<SecurityStampValidationOptions> options,
            ILogger<SecurityStampJwtBearerEvents> logger)
        {
            _userManager = userManager;
            _options = options.Value;
            _logger = logger;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                Fail(context, "JWT is missing the user identifier claim.", userId);
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                Fail(context, "JWT user was not found.", userId);
                return;
            }

            var tokenStamp = context.Principal?.FindFirst(SecurityStampValidationOptions.ClaimType)?.Value;
            if (string.IsNullOrWhiteSpace(tokenStamp))
            {
                if (IsWithinCompatibilityWindow(context.SecurityToken))
                {
                    _logger.LogDebug("Accepted a JWT without a security stamp for user {UserId} during the compatibility window.", userId);
                    return;
                }

                Fail(context, "JWT is missing the security stamp claim.", userId);
                return;
            }

            var currentStamp = await _userManager.GetSecurityStampAsync(user);
            if (!string.Equals(tokenStamp, currentStamp, StringComparison.Ordinal))
            {
                Fail(context, "JWT security stamp is no longer valid.", userId);
            }
        }

        private bool IsWithinCompatibilityWindow(object? securityToken)
        {
            if (securityToken is not JwtSecurityToken jwtToken)
            {
                return false;
            }

            var issuedAtClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Iat)?.Value;
            if (!long.TryParse(issuedAtClaim, out var issuedAtSeconds))
            {
                return false;
            }

            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(issuedAtSeconds);
            return issuedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(_options.CompatibilityWindowMinutes));
        }

        private void Fail(TokenValidatedContext context, string reason, string? userId)
        {
            _logger.LogWarning("JWT validation failed for user {UserId}: {Reason}", userId ?? "unknown", reason);
            context.Fail(reason);
        }
    }
}
