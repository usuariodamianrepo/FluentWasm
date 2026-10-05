namespace Backend.API.Options
{
    public sealed class ApiCorsOptions
    {
        public string[] AllowedOrigins { get; set; } = [];
    }

    public sealed class ApiForwardedHeadersOptions
    {
        public bool Enabled { get; set; }
        public int? ForwardLimit { get; set; } = 1;
        public string[] KnownNetworks { get; set; } = [];
        public string[] KnownProxies { get; set; } = [];
    }

    public sealed class ApiHealthEndpointOptions
    {
        public bool ExposeInProduction { get; set; }
        public string LivePath { get; set; } = "/alive";
        public string ReadyPath { get; set; } = "/health";
    }

    public sealed class ApiRuntimeOptions
    {
        public const string SectionName = "Runtime";
        public ApiCorsOptions Cors { get; set; } = new();
        public ApiForwardedHeadersOptions ForwardedHeaders { get; set; } = new();
        public ApiHealthEndpointOptions Health { get; set; } = new();
        public PublicApiRateLimitingOptions RateLimiting { get; set; } = new();
        public ApiSecurityOptions Security { get; set; } = new();
    }

    public sealed class ApiSecurityOptions
    {
        public bool EnableHsts { get; set; } = true;
        public bool EnableHttpsRedirection { get; set; } = true;
        public string RefreshTokenCookieName { get; set; } = "__Host-myapp-refresh";
        public string RefreshTokenCookieSameSite { get; set; } = "Strict";
        public int RefreshTokenLifetimeDays { get; set; } = 14;
    }

    public sealed class AuthEndpointRateLimitingOptions
    {
        public int PermitLimit { get; set; } = 5;
        public int QueueLimit { get; set; }
        public int WindowSeconds { get; set; } = 60;
    }

    public sealed class PublicApiRateLimitingOptions
    {
        public AuthEndpointRateLimitingOptions Auth { get; set; } = new();
        public bool Enabled { get; set; } = true;
        public int PermitLimit { get; set; } = 60;
        public int QueueLimit { get; set; }
        public int WindowSeconds { get; set; } = 60;
    }
}