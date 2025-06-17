using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Options;
using Microsoft.Extensions.Options;
using Shared.Abstractions.Interfaces;
using Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Services;

public class TestService : ITestService
{
    private readonly IHttpClientService _http;
    private readonly IOptions<ServiceUrlSettings> _options;
    private readonly IOptions<ApiKeySetting> _apiKeySettings;

    public TestService(
        IHttpClientService http, 
        IOptions<ServiceUrlSettings> options,
        IOptions<ApiKeySetting> apiKeySettings)
    {
        _http = http;
        _options = options;
        _apiKeySettings = apiKeySettings;
    }

    public async Task<TestMessageModel> TestAccountServiceAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.BaseAccountUrl}?id={id}");
        request.Headers.Add("InternalApiKey", _apiKeySettings.Value.ClientApiKey);

        return await _http.SendAsync<TestMessageModel>(request);
    }

    public async Task<TestMessageModel> TestAuthServiceAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.BaseAuthUrl}?id={id}");
        request.Headers.Add("InternalApiKey", _apiKeySettings.Value.ClientApiKey);

        return await _http.SendAsync<TestMessageModel>(request);
    }

    public async Task<TestMessageModel> TestSubscriptionServiceAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.BaseSubscriptionUrl}?id={id}");
        request.Headers.Add("InternalApiKey", _apiKeySettings.Value.ClientApiKey);

        return await _http.SendAsync<TestMessageModel>(request);
    }
}
