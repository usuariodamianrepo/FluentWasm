namespace Backend.Infrastructure
{
    using Backend.Application.DTOs;
    using Backend.Application.Services.Contracts.Admin;
    using Backend.Application.Services.Contracts.Logging;
    using Backend.Domain.Contracts;
    using Backend.Domain.Contracts.Authentication;
    using Backend.Domain.Entities.Identity;
    using Backend.Infrastructure.Configuration;
    using Backend.Infrastructure.Data;
    using Backend.Infrastructure.ExceptionsMiddleware;
    using Backend.Infrastructure.Repositories;
    using Backend.Infrastructure.Repositories.Authentication;
    using Backend.Infrastructure.Services;
    using Backend.Infrastructure.Services.Authentication;
    using Backend.Infrastructure.Services.Admin;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(
                opt => opt
                    .UseNpgsql(
                        config.GetConnectionString("DefaultConnection"),
                        npgsqlOptions =>
                            {
                                npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                                npgsqlOptions.EnableRetryOnFailure();
                            })
            );

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddDefaultIdentity<AppUser>(
                opt =>
                    {
                        opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                        opt.Lockout.AllowedForNewUsers = true;
                        opt.Lockout.MaxFailedAccessAttempts = 5;
                        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                        opt.Password.RequireDigit = true;
                        opt.Password.RequireNonAlphanumeric = true;
                        opt.Password.RequiredLength = 8;
                        opt.Password.RequireLowercase = true;
                        opt.Password.RequireUppercase = true;
                        opt.Password.RequiredUniqueChars = 1;
                    })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = config["JWT:Audience"],
                        ValidIssuer = config["JWT:Issuer"],
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                    };
                });

            services.AddOptions<SecurityStampValidationOptions>()
                .Bind(config.GetSection(SecurityStampValidationOptions.SectionName))
                .Validate(
                    options => options.CompatibilityWindowMinutes >= 0,
                    "JWT:CompatibilityWindowMinutes must be zero or greater.")
                .ValidateOnStart();
            services.AddScoped<SecurityStampJwtBearerEvents>();

            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<IAppTokenManager, AppTokenManager>();
            services.AddScoped<IAppRoleManager, AppRoleManager>();

            services.AddScoped<IAdminAuditService, AdminAuditService>();
            services.AddScoped<IAdminSettingsService, AdminSettingsService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IContactRepository, ContactRepository>();

            services.AddHttpContextAccessor();

            // Add memory cache for recommendations
            services.AddMemoryCache();


            services.AddSingleton<IValidateOptions<EmailSettings>, EmailSettingsOptionsValidator>();
            services.AddOptions<EmailSettings>()
                .Bind(config.GetSection("EmailSettings"))
                .ValidateOnStart();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
