namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Admin.Audit;

    public interface IAdminAuditService
    {
        Task<QueryResult<PagedResult<AdminAuditLog>>> GetAsync(AdminAuditQuery query);

        Task<QueryResult<AdminAuditLog>> GetByIdAsync(Guid id);
    }
}
