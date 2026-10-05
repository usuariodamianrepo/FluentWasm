namespace Frontend.Blazor.Authentication.Providers
{
    public sealed class AuthenticatedClientStateCleaner : IAuthenticatedClientStateCleaner
    {
        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }
    }
}