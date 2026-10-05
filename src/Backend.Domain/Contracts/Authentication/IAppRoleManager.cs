namespace Backend.Domain.Contracts.Authentication
{
    using Backend.Domain.Entities.Identity;

    public interface IAppRoleManager
    {
        Task<string?> GetUserRoleAsync(string userEmail);

        Task<bool> AddUserToRoleAsync(AppUser user, string roleName);
    }
}
