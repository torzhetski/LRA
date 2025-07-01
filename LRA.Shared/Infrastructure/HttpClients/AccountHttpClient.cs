using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class AccountHttpClient : IAccountHttpClient
{
    private  HttpClient _httpClient;
    private readonly string _apiKey;

    public AccountHttpClient(HttpClient httpClient, IOptions<AccountServiceSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<TestDto> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<TestDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model;
    }

    public async Task<TestMessageModel> PostAsync(TestDto model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post,"")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var message = JsonSerializer.Deserialize<TestMessageModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return message;
    }

    public async Task PostEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(messageModel);
        var request = new HttpRequestMessage(HttpMethod.Post, "brocker")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
