namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Seo;

    public interface ISeoRedirectService
    {
        Task<QueryResult<IReadOnlyList<GetSeoRedirect>>> GetAllAsync();

        Task<QueryResult<GetSeoRedirect>> GetByIdAsync(Guid redirectId);

        Task<ServiceResponse<GetSeoRedirect>> CreateAsync(UpsertSeoRedirect request);

        Task<ServiceResponse<GetSeoRedirect>> UpdateAsync(Guid redirectId, UpsertSeoRedirect request);

        Task<ServiceResponse<GetSeoRedirect>> DeactivateAsync(Guid redirectId);

        Task<ServiceResponse<GetSeoRedirect>> DeleteAsync(Guid redirectId);
    }
}