namespace Frontend.Blazor.Authentication.Providers
{
    public interface IAuthenticationSessionEventPublisher
    {
        Task PublishSignedInAsync();

        Task PublishSignedOutAsync();

        Task PublishSessionRefreshedAsync();
    }
}