namespace Backend.Infrastructure.Services.Admin
{
    using System.Globalization;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    using Backend.Application.DTOs;
    using Backend.Application.DTOs.Admin.Audit;
    using Backend.Application.DTOs.Admin.Settings;
    using Backend.Application.Services.Contracts.Admin;
    using Backend.Domain.Entities;
    using Backend.Infrastructure.Data;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public class AdminSettingsService : IAdminSettingsService
    {
        private static readonly Regex CurrencyRegex = new("^[A-Z]{3}$", RegexOptions.Compiled);

        private readonly AppDbContext _db;
        private readonly EmailSettings _emailSettings;
        private readonly IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminAuditService _auditService;

        public AdminSettingsService(
            AppDbContext db,
            IOptions<EmailSettings> emailSettings,
            IHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IAdminAuditService auditService)
        {
            _db = db;
            _emailSettings = emailSettings.Value;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _auditService = auditService;
        }

        public async Task<AdminSettingsDto> GetAsync()
        {
            var settings = await GetOrCreateAsync();
            return Map(settings);
        }

        public async Task<ServiceResponse<AppSettingsDto>> UpdateAppAsync(UpdateAppSettingsDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validationMessage = ValidateApp(request);
            if (validationMessage is not null)
            {
                return Failure<AppSettingsDto>(validationMessage, ServiceResponseType.ValidationError);
            }

            var settings = await GetOrCreateAsync();
            settings.AppName = request.AppName.Trim();
            settings.AppSupportEmail = Normalize(request.AppSupportEmail);
            settings.AppSupportPhone = Normalize(request.AppSupportPhone);
            settings.DefaultCurrency = request.DefaultCurrency.Trim().ToUpperInvariant();
            settings.DefaultCulture = request.DefaultCulture.Trim();
            settings.MaintenanceModeEnabled = request.MaintenanceModeEnabled;
            settings.MaintenanceMessage = Normalize(request.MaintenanceMessage);
            Touch(settings);

            await _db.SaveChangesAsync();
            await LogAsync("AdminSettings.StoreUpdated", "App settings updated.", settings);

            return Success(MapStore(settings), "App settings updated successfully.");
        }

        private async Task<AdminSettings> GetOrCreateAsync()
        {
            var settings = await _db.AdminSettings.FirstOrDefaultAsync();
            if (settings is not null)
            {
                return settings;
            }

            settings = new AdminSettings
            {
                SmtpHost = _emailSettings.SmtpServer,
                SmtpFromEmail = _emailSettings.From,
                SmtpFromDisplayName = _emailSettings.DisplayName,
                UpdatedOn = DateTime.UtcNow,
            };

            _db.AdminSettings.Add(settings);
            await _db.SaveChangesAsync();

            return settings;
        }

        private AdminSettingsDto Map(AdminSettings settings)
        {
            return new AdminSettingsDto
            {
                Store = MapStore(settings),
                System = new SystemSettingsDto
                {
                    UpdatedOn = settings.UpdatedOn,
                    UpdatedByUserId = settings.UpdatedByUserId,
                    RuntimeEnvironment = _environment.EnvironmentName,
                    FrameworkDescription = RuntimeInformation.FrameworkDescription,
                },
            };
        }

        private AppSettingsDto MapStore(AdminSettings settings)
        {
            return new AppSettingsDto
            {
                AppName = settings.AppName,
                AppSupportEmail = settings.AppSupportEmail,
                AppSupportPhone = settings.AppSupportPhone,
                DefaultCurrency = settings.DefaultCurrency,
                DefaultCulture = settings.DefaultCulture,
                MaintenanceModeEnabled = settings.MaintenanceModeEnabled,
                MaintenanceMessage = settings.MaintenanceMessage,
            };
        }

        private void Touch(AdminSettings settings)
        {
            settings.UpdatedOn = DateTime.UtcNow;
            settings.UpdatedByUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task LogAsync(string action, string summary, AdminSettings settings)
        {
            var metadata = JsonSerializer.Serialize(new
            {
                settings.AppName,
                settings.DefaultCurrency,
                settings.DefaultCulture,
                settings.DefaultShippingStatus,
                settings.OrderReferencePrefix,
                settings.MaintenanceModeEnabled,
            });

            await _auditService.LogAsync(new CreateAdminAuditLogDto
            {
                Action = action,
                EntityType = "AdminSettings",
                EntityId = settings.Id.ToString(),
                Summary = summary,
                MetadataJson = metadata,
            });
        }

        private static string? ValidateApp(UpdateAppSettingsDto request)
        {
            if (string.IsNullOrWhiteSpace(request.AppName))
            {
                return "App name is required.";
            }

            if (!string.IsNullOrWhiteSpace(request.AppSupportEmail) && !IsEmail(request.AppSupportEmail))
            {
                return "App support email is invalid.";
            }

            if (string.IsNullOrWhiteSpace(request.DefaultCurrency) || !CurrencyRegex.IsMatch(request.DefaultCurrency.Trim().ToUpperInvariant()))
            {
                return "Default currency must be a three-letter ISO currency code.";
            }

            if (string.IsNullOrWhiteSpace(request.DefaultCulture))
            {
                return "Default culture is required.";
            }

            try
            {
                CultureInfo.GetCultureInfo(request.DefaultCulture.Trim());
            }
            catch (CultureNotFoundException)
            {
                return "Default culture is invalid.";
            }

            return null;
        }

        private static bool IsEmail(string email)
        {
            try
            {
                _ = new MailAddress(email.Trim());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string Normalize(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        private static ServiceResponse<TPayload> Success<TPayload>(TPayload payload, string message)
        {
            return new ServiceResponse<TPayload>(true, message)
            {
                Payload = payload,
                ResponseType = ServiceResponseType.Success,
            };
        }

        private static ServiceResponse<TPayload> Failure<TPayload>(string message, ServiceResponseType responseType)
        {
            return new ServiceResponse<TPayload>(false, message)
            {
                ResponseType = responseType,
            };
        }
    }
}
