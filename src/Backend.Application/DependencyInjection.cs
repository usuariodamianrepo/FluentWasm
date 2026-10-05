namespace Backend.Application
{
    using Backend.Application.Options;
    using Backend.Application.Services;
    using Backend.Application.Services.Authentication;
    using Backend.Application.Services.Contracts;
    using Backend.Application.Services.Contracts.Authentication;
    using Backend.Application.Validations;
    using Backend.Application.Validations.Authentication;
    using FluentValidation;
    using Mapster;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapster();

            services.AddOptions<ClientAppOptions>()
                .Bind(configuration.GetSection(ClientAppOptions.SectionName))
                .Validate(
                    options => !string.IsNullOrWhiteSpace(options.BaseUrl),
                    $"{ClientAppOptions.SectionName}:BaseUrl is required.")
                .Validate(
                    options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _),
                    $"{ClientAppOptions.SectionName}:BaseUrl must be an absolute URL.")
                .ValidateOnStart();

            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IContactService, ContactService>();

            return services;
        }
    }
}
