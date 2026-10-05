namespace Backend.Tests.Infrastructure.Authentication
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    using Backend.Domain.Entities.Identity;
    using Backend.Infrastructure.Configuration;
    using Backend.Infrastructure.Services.Authentication;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class SecurityStampJwtBearerEventsTests
    {
        private const string UserId = "user-1";
        private const string CurrentStamp = "current-stamp";

        [Fact]
        public async Task TokenValidated_WithMatchingStamp_Succeeds()
        {
            var user = CreateUser();
            var events = CreateEvents(user);
            var context = CreateContext(CreatePrincipal(UserId, CurrentStamp), DateTimeOffset.UtcNow);

            await events.TokenValidated(context);

            Assert.Null(context.Result);
        }

        [Fact]
        public async Task TokenValidated_WithDifferentStamp_Fails()
        {
            var user = CreateUser();
            var events = CreateEvents(user);
            var context = CreateContext(CreatePrincipal(UserId, "old-stamp"), DateTimeOffset.UtcNow);

            await events.TokenValidated(context);

            Assert.NotNull(context.Result?.Failure);
        }

        [Fact]
        public async Task TokenValidated_WithUnknownUser_Fails()
        {
            var events = CreateEvents(null);
            var context = CreateContext(CreatePrincipal("missing-user", CurrentStamp), DateTimeOffset.UtcNow);

            await events.TokenValidated(context);

            Assert.NotNull(context.Result?.Failure);
        }

        [Fact]
        public async Task TokenValidated_WithoutUserIdentifier_Fails()
        {
            var events = CreateEvents(null);
            var context = CreateContext(CreatePrincipal(null, CurrentStamp), DateTimeOffset.UtcNow);

            await events.TokenValidated(context);

            Assert.NotNull(context.Result?.Failure);
        }

        [Fact]
        public async Task TokenValidated_WithoutStamp_WithinCompatibilityWindow_Succeeds()
        {
            var user = CreateUser();
            var events = CreateEvents(user, compatibilityWindowMinutes: 10);
            var context = CreateContext(CreatePrincipal(UserId, null), DateTimeOffset.UtcNow.AddMinutes(-5));

            await events.TokenValidated(context);

            Assert.Null(context.Result);
        }

        [Fact]
        public async Task TokenValidated_WithoutStamp_OutsideCompatibilityWindow_Fails()
        {
            var user = CreateUser();
            var events = CreateEvents(user, compatibilityWindowMinutes: 10);
            var context = CreateContext(CreatePrincipal(UserId, null), DateTimeOffset.UtcNow.AddMinutes(-11));

            await events.TokenValidated(context);

            Assert.NotNull(context.Result?.Failure);
        }

        private static SecurityStampJwtBearerEvents CreateEvents(AppUser? user, int compatibilityWindowMinutes = 120)
        {
            var userManager = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<AppUser>(),
                Array.Empty<IUserValidator<AppUser>>(),
                Array.Empty<IPasswordValidator<AppUser>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                new ServiceCollection().BuildServiceProvider(),
                NullLogger<UserManager<AppUser>>.Instance);
            userManager
                .Setup(manager => manager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userManager
                .Setup(manager => manager.GetSecurityStampAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(CurrentStamp);

            return new SecurityStampJwtBearerEvents(
                userManager.Object,
                Options.Create(new SecurityStampValidationOptions
                {
                    CompatibilityWindowMinutes = compatibilityWindowMinutes
                }),
                NullLogger<SecurityStampJwtBearerEvents>.Instance);
        }

        private static TokenValidatedContext CreateContext(ClaimsPrincipal principal, DateTimeOffset issuedAt)
        {
            var token = new JwtSecurityToken(
                claims: principal.Claims.Append(new Claim(
                    JwtRegisteredClaimNames.Iat,
                    issuedAt.ToUnixTimeSeconds().ToString(System.Globalization.CultureInfo.InvariantCulture))),
                expires: issuedAt.UtcDateTime.AddHours(1));
            var scheme = new AuthenticationScheme(
                JwtBearerDefaults.AuthenticationScheme,
                JwtBearerDefaults.AuthenticationScheme,
                typeof(JwtBearerHandler));

            var context = new TokenValidatedContext(new DefaultHttpContext(), scheme, new JwtBearerOptions());
            context.Principal = principal;
            context.SecurityToken = token;
            return context;
        }

        private static ClaimsPrincipal CreatePrincipal(string? userId, string? securityStamp)
        {
            var claims = new List<Claim>();
            if (userId is not null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            if (securityStamp is not null)
            {
                claims.Add(new Claim(SecurityStampValidationOptions.ClaimType, securityStamp));
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
        }

        private static AppUser CreateUser()
        {
            return new AppUser
            {
                Id = UserId,
                UserName = "user@example.com",
                SecurityStamp = CurrentStamp
            };
        }
    }
}
