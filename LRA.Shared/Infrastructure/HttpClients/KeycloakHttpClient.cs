using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class KeycloakHttpClient : IKeycloakHttpClient
{
    private HttpClient _httpClient;
    private readonly IOptions<KeycloakServiceSettings> _options;
    private readonly ILogger<KeycloakHttpClient> _logger;

    public KeycloakHttpClient(
        HttpClient httpClient, 
        IOptions<KeycloakServiceSettings> options, 
        ILogger<KeycloakHttpClient> logger)
    {
        _httpClient = httpClient;
        _options = options;
        _logger = logger;
    }

    public async Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.KeycloakBaseUrl}.well-known/openid-configuration");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var _userManagementToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.AdminBaseUrl}users");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userManagementToken);

        var userData = new
        {
            email = model.Email,
            username = model.Email,
            enabled = true,
            credentials = new[]
            {
            new {
                type = "password",
                value = model.Password,
                temporary = false
                }
            }
        };

        var json = JsonSerializer.Serialize(userData);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

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

        var location = response.Headers.Location?.ToString();
        var userId = location?.Split('/').Last();

        return userId!;
    }

    private async Task<string> GetAdminAccessTokenByPasswordAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.AccessTokenUrl}");

        var body = new Dictionary<string, string>
        {
            ["client_id"] = $"{_options.Value.ClientId}",
            ["username"] = $"{_options.Value.Username}",
            ["password"] = $"{_options.Value.Password}",
            ["grant_type"] = $"password"
        };

        request.Content = new FormUrlEncodedContent(body);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);
        var accessToken = doc.RootElement.GetProperty("access_token").GetString();

        return accessToken;
    }
}
