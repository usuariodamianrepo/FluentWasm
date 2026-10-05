namespace Backend.Application.Services.Contracts.Admin
{
    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Settings;

    public interface IAdminSettingsService
    {
        Task<AdminSettingsDto> GetAsync();

        Task<ServiceResponse<AppSettingsDto>> UpdateAppAsync(UpdateAppSettingsDto request);

    }
}
