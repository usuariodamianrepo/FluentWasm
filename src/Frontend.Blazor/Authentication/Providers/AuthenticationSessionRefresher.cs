namespace Frontend.Blazor.Authentication.Providers
{
    using System.Net;

    using Frontend.Shared;
    using Frontend.Shared.Helper.Contracts;
    using Frontend.Shared.Models;
    using Frontend.Shared.Services.Contracts;

    public sealed class AuthenticationSessionRefresher : IAuthenticationSessionRefresher
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;
        //private readonly IAuthenticationStateNotifier _authenticationStateNotifier;
        private readonly IAuthenticationSessionEventPublisher _sessionEventPublisher;

        public AuthenticationSessionRefresher(
            ITokenService tokenService,
            IAuthenticationService authenticationService,
            IAuthenticationSessionEventPublisher sessionEventPublisher)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
            _sessionEventPublisher = sessionEventPublisher;
        }

        public async Task<LoginResponse?> TryRefreshAsync(bool clearTokenOnFailure = true)
        {
            var result = await _authenticationService.ReviveToken();

            if (result.Success && !string.IsNullOrWhiteSpace(result.Data?.Token))
            {
                await _tokenService.StoreJwtTokenAsync(Constant.TokenStorage.Key, result.Data.Token);
                //_authenticationStateNotifier.NotifyAuthenticationState();
                return result.Data;
            }

            if (clearTokenOnFailure && ShouldClearToken(result.StatusCode))
            {
                await _tokenService.RemoveJwtTokenAsync(Constant.TokenStorage.Key);
                //_authenticationStateNotifier.NotifyAuthenticationState();
                await _sessionEventPublisher.PublishSignedOutAsync();
            }

            return null;
        }

        private static bool ShouldClearToken(HttpStatusCode? statusCode)
        {
            return statusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden;
        }
    }
}