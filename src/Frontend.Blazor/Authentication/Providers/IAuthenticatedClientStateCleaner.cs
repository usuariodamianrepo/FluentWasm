namespace Frontend.Blazor.Authentication.Providers
{
    public interface IAuthenticatedClientStateCleaner
    {
        Task ClearAsync();
    }
}