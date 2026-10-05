using Frontend.Blazor;
using Frontend.Blazor.Authentication.Providers;
using Frontend.Shared;
using Frontend.Shared.BrowserStorage;
using Frontend.Shared.BrowserStorage.Contracts;
using Frontend.Shared.CookieStorage;
using Frontend.Shared.CookieStorage.Contracts;
using Frontend.Shared.Helper;
using Frontend.Shared.Helper.Contracts;
using Frontend.Shared.Services;
using Frontend.Shared.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseAddress = await ResolveApiBaseAddressAsync(builder);

builder.Services.AddSingleton<IBrowserCookieStorageService, BrowserCookieStorageService>();
builder.Services.AddSingleton<IBrowserSessionStorageService, BrowserSessionStorageService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHttpClientHelper, HttpClientHelper>();
builder.Services.AddScoped<IApiCallHelper, ApiCallHelper>();
builder.Services.AddScoped<ISeoSettingsService, SeoSettingsService>();
builder.Services.AddScoped<ISeoRedirectService, SeoRedirectService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminSettingsService, AdminSettingsService>();
builder.Services.AddScoped<IAdminAuditService, AdminAuditService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthenticationStateNotifier, AuthenticationStateNotifier>();
builder.Services.AddScoped<IAuthenticatedClientStateCleaner, AuthenticatedClientStateCleaner>();
builder.Services.AddScoped<IAuthenticationSessionEventPublisher, AuthenticationSessionEventPublisher>();
builder.Services.AddScoped<IAuthenticationSessionRefresher, AuthenticationSessionRefresher>();

builder.Services.AddScoped<BrowserCredentialsHandler>();

builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddHttpClient(
    Constant.ApiClient.PublicName,
    client => { client.BaseAddress = apiBaseAddress; }
).AddHttpMessageHandler<BrowserCredentialsHandler>();
builder.Services.AddHttpClient(
    Constant.ApiClient.PrivateName,
    client => { client.BaseAddress = apiBaseAddress; }
).AddHttpMessageHandler<RefreshTokenHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();


builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();

static async Task<Uri> ResolveApiBaseAddressAsync(WebAssemblyHostBuilder builder)
{
    var relativeApiBaseAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/");

    if (await IsRelativeApiAvailableAsync(relativeApiBaseAddress))
    {
        return relativeApiBaseAddress;
    }

    var configuredBaseAddress = builder.Configuration["Api:DirectBaseUrl"];
    if (!string.IsNullOrWhiteSpace(configuredBaseAddress) &&
        Uri.TryCreate(configuredBaseAddress, UriKind.Absolute, out var configuredUri))
    {
        return configuredUri;
    }

    return relativeApiBaseAddress;
}

static async Task<bool> IsRelativeApiAvailableAsync(Uri relativeApiBaseAddress)
{
    using var httpClient = new HttpClient();
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

    try
    {
        using var response = await httpClient.GetAsync(new Uri(relativeApiBaseAddress, "openapi/v1.json"), cts.Token);
        var mediaType = response.Content.Headers.ContentType?.MediaType;

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        return IsJsonMediaType(mediaType);
    }
    catch (HttpRequestException)
    {
        return false;
    }
    catch (OperationCanceledException)
    {
        return false;
    }
}

static bool IsJsonMediaType(string? mediaType)
{
    if (string.IsNullOrWhiteSpace(mediaType))
    {
        return false;
    }

    return string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase)
           || mediaType.EndsWith("+json", StringComparison.OrdinalIgnoreCase);
}