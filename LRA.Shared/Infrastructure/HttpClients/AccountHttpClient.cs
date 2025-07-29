using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.WebUtilities;
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

    public async Task<AccountDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<AccountDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model;
    }

    public async Task<PaginatedList<AccountDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("paginated", new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString()
        });

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<PaginatedList<AccountDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model;
    }

    public async Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<List<AccountDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model;
    }

    public async Task CreateAccountAsync(BaseAccountDto dto, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(dto);
        var request = new HttpRequestMessage(HttpMethod.Post, "create-user")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAccountAsync(AccountDto dto, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(dto);
        var request = new HttpRequestMessage(HttpMethod.Put, "update-user")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTwoFaAsync(Guid keycloakid, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("update-twofa", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakid.ToString(),
        });
        var request = new HttpRequestMessage(HttpMethod.Put, url);
        
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"soft/{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
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

    public async Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("verify-email", new Dictionary<string, string?>
        {
            ["token"] = token,
            ["email"] = email
        });

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> IsTwoFaEnabledAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"two-fa/{keycloakId}");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonSerializer.Deserialize<GenericServiceResponse<bool>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return model.Response;
    }
}
