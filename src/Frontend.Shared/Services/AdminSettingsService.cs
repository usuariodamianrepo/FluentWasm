namespace Frontend.Shared.Services
{
    using Frontend.Shared.Helper.Contracts;
    using Frontend.Shared.Models;
    using Frontend.Shared.Models.Admin.Settings;
    using Frontend.Shared.Services.Contracts;

    public class AdminSettingsService : IAdminSettingsService
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IApiCallHelper _apiCallHelper;

        public AdminSettingsService(IHttpClientHelper httpClientHelper, IApiCallHelper apiCallHelper)
        {
            _httpClientHelper = httpClientHelper;
            _apiCallHelper = apiCallHelper;
        }

        public async Task<QueryResult<AdminSettingsModel>> GetAsync()
        {
            var result = await SendAsync<Unit>(Constant.AdminSettings.Base, Constant.ApiCallType.Get);
            return await _apiCallHelper.GetQueryResult<AdminSettingsModel>(result, "We couldn't load admin settings right now. Please try again.");
        }

        private async Task<HttpResponseMessage> SendAsync<TModel>(string route, string type, TModel? model = default)
        {
            var client = await _httpClientHelper.GetPrivateClientAsync();
            return await _apiCallHelper.ApiCallTypeCall<TModel>(new ApiCall
            {
                Client = client,
                Route = route,
                Type = type,
                Model = model,
            });
        }
    }
}
