namespace Backend.Application.DTOs.Admin.Settings
{
    public class AppSettingsDto
    {
        public string AppName { get; set; } = string.Empty;

        public string AppSupportEmail { get; set; } = string.Empty;

        public string AppSupportPhone { get; set; } = string.Empty;

        public string DefaultCurrency { get; set; } = "EUR";

        public string DefaultCulture { get; set; } = "en-US";

        public bool MaintenanceModeEnabled { get; set; }

        public string MaintenanceMessage { get; set; } = string.Empty;
    }
}
