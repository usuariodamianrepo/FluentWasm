namespace Backend.Application.Services.Contracts.Admin
{
    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Users;
    using Backend.Domain.Contracts;

    public interface IAdminUserService
    {
        Task<PagedResult<AdminUserListItemDto>> GetUsersAsync(AdminUserQueryDto query);

        Task<IReadOnlyList<string>> GetRolesAsync();

        Task<ServiceResponse<AdminUserDetailsDto>> GetByIdAsync(string id);

        Task<ServiceResponse<AdminUserDetailsDto>> UpdateRolesAsync(string id, UpdateUserRolesDto request, string? currentAdminUserId);

        Task<ServiceResponse<AdminUserDetailsDto>> LockAsync(string id, UserLockRequestDto request, string? currentAdminUserId);

        Task<ServiceResponse<AdminUserDetailsDto>> UnlockAsync(string id);

        Task<ServiceResponse<AdminUserDetailsDto>> ConfirmEmailAsync(string id);

        Task<ServiceResponse<AdminUserDetailsDto>> RequirePasswordChangeAsync(string id);

        Task<ServiceResponse<AdminUserDetailsDto>> DeactivateAsync(string id, string? currentAdminUserId);
    }
}
