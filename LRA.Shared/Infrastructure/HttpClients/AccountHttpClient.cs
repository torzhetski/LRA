using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;

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

    public async Task<AccountDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{id}");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<AccountDetailsDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task<AccountDetailsDto> GetByKeycloakIdAsync(string keycloakId, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"keycloak/{keycloakId}");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<AccountDetailsDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task<PaginatedList<AccountSummaryDto>> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        var url = QueryHelpers.AddQueryString("filtrated", new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString()
        });
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<PaginatedList<AccountSummaryDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return items;
    }

    public async Task<PaginatedList<AccountSummaryDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
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

        var model = await response.Content.ReadFromJsonAsync<PaginatedList<AccountSummaryDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return model;
    }

    public async Task<List<AccountSummaryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"all");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var model = await response.Content.ReadFromJsonAsync<List<AccountSummaryDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

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

    public async Task SoftDeleteAsync(Guid id, string keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString($"soft/{id}", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakId,
        });
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    throw new PermissionException(response.RequestMessage.ToString());
                default:
                    throw new Exception("deletion failed");
            }
        }
    }

    public async Task DeleteAsync(Guid id, string keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString($"{id}", new Dictionary<string, string?>
        {
            ["keycloakId"] = keycloakId,
        });
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    throw new PermissionException(response.RequestMessage.ToString());
                default:
                    throw new Exception("deletion failed");
            }
        }
    }

    public async Task<List<string>> GetRolesByKeycloakId(string keycloakId, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{keycloakId}/roles");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var roles = await response.Content.ReadFromJsonAsync<List<string>>();

        return roles;
    }
}
