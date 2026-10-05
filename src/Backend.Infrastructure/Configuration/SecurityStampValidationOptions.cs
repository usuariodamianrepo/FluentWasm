namespace Backend.Infrastructure.Configuration
{
    public sealed class SecurityStampValidationOptions
    {
        public const string SectionName = "JWT";
        public const string ClaimType = "security_stamp";

        public int CompatibilityWindowMinutes { get; set; } = 120;
    }
}
