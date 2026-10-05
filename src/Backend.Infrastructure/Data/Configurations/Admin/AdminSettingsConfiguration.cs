namespace Backend.Infrastructure.Data.Configurations.Admin
{
    using Backend.Domain.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AdminSettingsConfiguration : IEntityTypeConfiguration<AdminSettings>
    {
        public void Configure(EntityTypeBuilder<AdminSettings> builder)
        {
            builder.Property(settings => settings.AppName)
                .HasMaxLength(160)
                .IsRequired();

            builder.Property(settings => settings.AppSupportEmail)
                .HasMaxLength(256);

            builder.Property(settings => settings.AppSupportPhone)
                .HasMaxLength(64);

            builder.Property(settings => settings.DefaultCurrency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(settings => settings.DefaultCulture)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(settings => settings.MaintenanceMessage)
                .HasMaxLength(1000);

            builder.Property(settings => settings.DefaultShippingStatus)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(settings => settings.OrderReferencePrefix)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(settings => settings.SmtpHost)
                .HasMaxLength(256);

            builder.Property(settings => settings.SmtpFromEmail)
                .HasMaxLength(256);

            builder.Property(settings => settings.SmtpFromDisplayName)
                .HasMaxLength(160);

            builder.Property(settings => settings.UpdatedByUserId)
                .HasMaxLength(450);

            builder.Property(settings => settings.UpdatedOn)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
