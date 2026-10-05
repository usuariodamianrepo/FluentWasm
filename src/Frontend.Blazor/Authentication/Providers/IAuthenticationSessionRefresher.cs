namespace Frontend.Blazor.Authentication.Providers
{
    using Frontend.Shared.Models;

    public interface IAuthenticationSessionRefresher
    {
        Task<LoginResponse?> TryRefreshAsync(bool clearTokenOnFailure = true);
    }
}