using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class KeycloakHttpClient : IKeycloakHttpClient
{
    private HttpClient _httpClient;
    private readonly string _userManagementToken;

    public KeycloakHttpClient(HttpClient httpClient, IOptions<KeycloakServiceSettings> options)
    {
        _httpClient = httpClient;
        _userManagementToken = options.Value.ApiKey;
    }

    public async Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $".well-known/openid-configuration");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/admin/realms/keycloak-auth/users");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userManagementToken);

        var userData = new
        {
            model.Email,
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
        response.EnsureSuccessStatusCode();

        var location = response.Headers.Location?.ToString();
        var userId = location?.Split('/').Last();

        return userId!;
    }
}
