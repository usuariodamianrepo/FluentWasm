namespace Frontend.Shared.Services.Contracts
{
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Admin.Settings;

    public interface IAdminSettingsService
    {
        Task<QueryResult<AdminSettingsModel>> GetAsync();
    }
}
