using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class SubscriptionHttpClient : ISubscriptionHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public SubscriptionHttpClient(HttpClient httpClient, IOptions<SubscriptionServiceSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }


    public async Task<RedirectUrlModel> GetChekoutSessionAsync(string keycloakId, string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("chekout-session", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakId,
            ["email"] = email
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var redirectUrl = await response.Content.ReadFromJsonAsync<RedirectUrlModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return redirectUrl;
    }

    public async Task<RedirectUrlModel> GetCustomerPortalSessionAsync(string keycloakId, string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("customers-portal-session", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakId,
            ["email"] = email
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var redirectUrl = await response.Content.ReadFromJsonAsync<RedirectUrlModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return redirectUrl;
    }
}
