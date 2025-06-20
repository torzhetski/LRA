using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Shared.Abstractions.Test;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class AccountHttpClient : IAccountHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AccountHttpClient(HttpClient httpClient, IOptions<ApiKeySetting> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.AccountApiKey;
    }

    public async Task<TestMessageModel> GetAsync(int id, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"?id={id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<TestMessageModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model;
    }
}
