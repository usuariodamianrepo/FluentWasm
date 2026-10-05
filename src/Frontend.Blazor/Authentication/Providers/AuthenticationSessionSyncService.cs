namespace Frontend.Blazor.Authentication.Providers
{
    using Frontend.Shared;
    using Frontend.Shared.Helper.Contracts;

    using Microsoft.JSInterop;

    public sealed class AuthenticationSessionSyncService : IAuthenticationSessionSyncService
    {
        private const string ModulePath = "./js/authSessionSync.js";

        private readonly IAuthenticationSessionRefresher _sessionRefresher;
        private readonly ITokenService _tokenService;
        //private readonly IAuthenticationStateNotifier _authenticationStateNotifier;
        private readonly IJSRuntime _jsRuntime;

        private IJSObjectReference? _module;
        private DotNetObjectReference<AuthenticationSessionSyncService>? _selfReference;

        public AuthenticationSessionSyncService(
            IAuthenticationSessionRefresher sessionRefresher,
            ITokenService tokenService,
            IJSRuntime jsRuntime)
        {
            _sessionRefresher = sessionRefresher;
            _tokenService = tokenService;
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            if (_module is not null)
            {
                return;
            }

            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", ModulePath);
            _selfReference = DotNetObjectReference.Create(this);
            await _module.InvokeVoidAsync("subscribe", _selfReference);
        }

        [JSInvokable]
        public async Task HandleAuthSessionEventAsync(string eventType)
        {
            switch (eventType)
            {
                case "signed-in":
                case "session-refreshed":
                    await _sessionRefresher.TryRefreshAsync(clearTokenOnFailure: false);
                    break;
                case "signed-out":
                    await _tokenService.RemoveJwtTokenAsync(Constant.TokenStorage.Key);
                    //_authenticationStateNotifier.NotifyAuthenticationState();
                    break;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_module is not null)
            {
                try
                {
                    await _module.InvokeVoidAsync("unsubscribe");
                    await _module.DisposeAsync();
                }
                catch
                {
                    // Disposal should never block app shutdown.
                }
            }

            _selfReference?.Dispose();
        }
    }
}