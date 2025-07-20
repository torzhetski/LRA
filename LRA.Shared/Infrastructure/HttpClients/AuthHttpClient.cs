using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using LRA.Shared.Abstractions.Test;
using System.Text.Json;
using LRA.Shared.Abstractions.Models;
using System.Text;
using LRA.Shared.Utils.Exceptions;
using System.Net;
using System.Numerics;
using Microsoft.Extensions.Logging;

namespace LRA.Shared.Infrastructure.HttpClients;

public class AuthHttpClient : IAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthHttpClient> _logger;
    private readonly string _apiKey;

    public AuthHttpClient(
        HttpClient httpClient, 
        IOptions<AuthServiceSettings> options, 
        ILogger<AuthHttpClient> logger)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
        _logger = logger;
    }

    public async Task<string> GetRealmConfigAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"keycloak-config");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post, "keycloak")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Conflict)
            {
                throw new AlreadyExistsException("User");
            }
            else
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }
}
