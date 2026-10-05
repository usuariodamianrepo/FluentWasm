namespace Frontend.Blazor.Authentication.Providers
{
    public interface IAuthenticationSessionBootstrapper
    {
        Task RestoreAsync();
    }
}