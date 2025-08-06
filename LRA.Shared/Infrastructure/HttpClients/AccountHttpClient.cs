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

    public AccountHttpClient(HttpClient httpClient, IOptions<AccountingEndpointsSettings> options)
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"all");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<List<AccountDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto, string keycloakId, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(dto);
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{keycloakId}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTwoFaAsync(string keycloakId, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"twofactor/{keycloakId}");
        
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
}
