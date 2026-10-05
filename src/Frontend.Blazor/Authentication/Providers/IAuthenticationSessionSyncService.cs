namespace Frontend.Blazor.Authentication.Providers
{
    public interface IAuthenticationSessionSyncService : IAsyncDisposable
    {
        Task InitializeAsync();
    }
}