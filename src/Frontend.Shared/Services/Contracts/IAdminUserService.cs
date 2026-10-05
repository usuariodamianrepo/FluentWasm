namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Admin.Users;

    public interface IAdminUserService
    {
        Task<QueryResult<PagedResult<AdminUserListItem>>> GetUsersAsync(AdminUserQuery query);

        Task<QueryResult<IReadOnlyList<string>>> GetRolesAsync();

        Task<QueryResult<AdminUserDetails>> GetByIdAsync(string id);

        Task<ServiceResponse<AdminUserDetails>> UpdateRolesAsync(string id, UpdateUserRoles request);

        Task<ServiceResponse<AdminUserDetails>> LockAsync(string id, UserLockRequest request);

        Task<ServiceResponse<AdminUserDetails>> UnlockAsync(string id);

        Task<ServiceResponse<AdminUserDetails>> ConfirmEmailAsync(string id);

        Task<ServiceResponse<AdminUserDetails>> RequirePasswordChangeAsync(string id);

        Task<ServiceResponse<AdminUserDetails>> DeactivateAsync(string id);
    }
}
