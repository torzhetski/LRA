using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class KeycloakHttpClient : IKeycloakHttpClient
{
    private HttpClient _httpClient;
    private readonly IOptions<KeycloakServiceSettings> _options;

    public KeycloakHttpClient(
        HttpClient httpClient,
        IOptions<KeycloakServiceSettings> options)
    {
        _httpClient = httpClient;
        _options = options;
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

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new AlreadyExistsException("User");
                default:
                    throw new Exception("smthing went wrong");
            }

        var location = response.Headers.Location?.ToString();
        var userId = location?.Split('/').Last();

        return userId!;
    }

    public async Task<KeycloakTokenDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/token");

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = $"public-client",
            ["username"] = model.Email,
            ["password"] = model.Password,
        };

        request.Content = new FormUrlEncodedContent(body);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid user credentials");
                default:
                    throw new Exception("Failed to login");
            }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var token = JsonSerializer.Deserialize<KeycloakTokenDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return token;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/token");

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = "public-client",
            ["refresh_token"] = refreshTokenModel.RefreshToken
        };

        request.Content = new FormUrlEncodedContent(body);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
                default:
                    throw new Exception("Failed to refresh token");
            }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var token = JsonSerializer.Deserialize<KeycloakTokenDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return token;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var body = new Dictionary<string, string>
        {
            ["client_id"] = "public-client",
            ["refresh_token"] = refreshTokenModel.RefreshToken
        };
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/logout");

        request.Content = new FormUrlEncodedContent(body);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Logout failed: {response.StatusCode}. {content}");
        }
    }

    public async Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken)
    {
        var accessToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        var passwordPayload = new
        {
            type = "password",
            value = newPassword,
            temporary = false
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"{_options.Value.AdminBaseUrl}users/{keycloakId}/reset-password")
        {
            Content = JsonContent.Create(passwordPayload)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Password update failed: {response.StatusCode}");
    }

    public async Task ChangeUserEmailAsync(Guid keycloakId, string newEmail, CancellationToken cancellationToken)
    {
        var accessToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        var updatePayload = new
        {
            email = newEmail,
            username = newEmail,
            emailVerified = false
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"{_options.Value.AdminBaseUrl}users/{keycloakId}")
        {
            Content = JsonContent.Create(updatePayload)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to update email: {response.StatusCode}");
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
