namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Seo;

    public interface ISeoSettingsService
    {
        Task<QueryResult<GetSeoSettings>> GetAsync();

        Task<ServiceResponse<GetSeoSettings>> UpdateAsync(UpdateSeoSettings request);
    }
}