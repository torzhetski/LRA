using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<AccountDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task<PaginatedList<AccountDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("paginated", new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString()
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<PaginatedList<AccountDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<List<AccountDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task CreateAccountAsync(BaseAccountDto dto, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(dto);
        using var request = new HttpRequestMessage(HttpMethod.Post, "create-user")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAccountAsync(AccountDto dto, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(dto);
        using var request = new HttpRequestMessage(HttpMethod.Put, "update-user")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("update-twofa", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakId.ToString(),
        });
        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"soft/{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task PostEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(messageModel);
        using var request = new HttpRequestMessage(HttpMethod.Post, "brocker")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("verify-email", new Dictionary<string, string?>
        {
            ["token"] = token,
            ["email"] = email
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
