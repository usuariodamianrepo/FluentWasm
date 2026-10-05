namespace Backend.Application.DTOs.Admin.Settings
{
    public class AdminSettingsDto
    {
        public AppSettingsDto Store { get; set; } = new();

        public SystemSettingsDto System { get; set; } = new();
    }
}
