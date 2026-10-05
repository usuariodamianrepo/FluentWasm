namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Authentication;

    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUser user);

        Task<LoginResponse> LoginUser(LoginUser user);

        Task<DemoSessionModel> StartDemoSession(string role);

        Task<QueryResult<LoginResponse>> ReviveToken();

        Task<ServiceResponse> Logout();

        Task<ServiceResponse> ChangePassword(PasswordChangeModel changePasswordDto);

        Task<ServiceResponse> ConfirmEmail(string userId, string token);

        Task<ServiceResponse> UpdateProfile(UpdateProfileModel model);
    }
}
