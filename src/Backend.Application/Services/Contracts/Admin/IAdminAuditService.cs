namespace Backend.Application.Services.Contracts.Admin
{
    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Audit;
    using Backend.Domain.Contracts;

    public interface IAdminAuditService
    {
        Task<PagedResult<AdminAuditLogDto>> GetAsync(AdminAuditQueryDto query);

        Task<ServiceResponse<AdminAuditLogDto>> GetByIdAsync(Guid id);

        Task<ServiceResponse<AdminAuditLogDto>> LogAsync(CreateAdminAuditLogDto request);
    }
}
