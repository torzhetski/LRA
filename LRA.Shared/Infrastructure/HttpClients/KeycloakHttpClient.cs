using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Models.Keycloak;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
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
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.Value.KeycloakBaseUrl}.well-known/openid-configuration");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var _userManagementToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.AdminBaseUrl}users");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userManagementToken);

        var userData = new KeycloakUserCreateModel
        {
            Email = model.Email,
            Username = model.Email,
            Enabled = false,
            Credentials = new List<KeycloakCredentialModel>()
            {
            new KeycloakCredentialModel() {
                    Type = "password",
                    Value = model.Password,
                    Temporary = false
                }
            }
        };

        var json = JsonSerializer.Serialize(userData);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new AlreadyExistsException("Account");
                default:
                    throw new Exception("Registration failed");
            }
        }

        var location = response.Headers.Location?.ToString();
        var userId = location?.Split('/').Last();

        return userId!;
    }

    public async Task EnableUserAsync(Guid keycloakUserId, CancellationToken cancellationToken)
    {
        var adminToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        using var request = new HttpRequestMessage(HttpMethod.Put,$"{_options.Value.AdminBaseUrl}users/{keycloakUserId}")
        {
            Content = 
            JsonContent.Create(
                new KeycloakUserEnablingModel
                {
                    Enabled = true
                })
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Failed to enable user: {response.StatusCode}. {error}");
        }
    }

    public async Task<KeycloakTokenDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/token");

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = _options.Value.PublicClientId,
            ["username"] = model.Email,
            ["password"] = model.Password,
        };

        request.Content = new FormUrlEncodedContent(body);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid user credentials");
                case HttpStatusCode.BadRequest:
                    throw new InvalidVerificationException();
                default:
                    throw new Exception("Failed to login");
            }
        }

        var token = await response.Content.ReadFromJsonAsync<KeycloakTokenDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return token;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/token");

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = _options.Value.PublicClientId,
            ["refresh_token"] = refreshTokenModel.RefreshToken
        };

        request.Content = new FormUrlEncodedContent(body);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token" );
                default:
                    throw new Exception("Failed to refresh token");
            }
        }

        var token = await response.Content.ReadFromJsonAsync<KeycloakTokenDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return token;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var body = new Dictionary<string, string>
        {
            ["client_id"] = _options.Value.PublicClientId,
            ["refresh_token"] = refreshTokenModel.RefreshToken
        };
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/logout");

        request.Content = new FormUrlEncodedContent(body);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Logout failed: {response.StatusCode}. {content}");
        }
    }

    public async Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken)
    {
        var accessToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        var passwordPayload = new KeycloakCredentialModel
        {
            Type = "password",
            Value = newPassword,
            Temporary = false
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"{_options.Value.AdminBaseUrl}users/{keycloakId}/reset-password")
        {
            Content = JsonContent.Create(passwordPayload)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Password update failed: {response.StatusCode}");
        }
    }

    public async Task ChangeUserEmailAsync(Guid keycloakId, string newEmail, CancellationToken cancellationToken)
    {
        var accessToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        var updatePayload = new UpdateKeycloakEmailModel
        {
            Email = newEmail,
            Username = newEmail,
            EmailVerified = true,
            Enabled = true
        };

        using var request = new HttpRequestMessage(HttpMethod.Put, $"{_options.Value.AdminBaseUrl}users/{keycloakId}")
        {
            Content = JsonContent.Create(updatePayload)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update email: {response.StatusCode}");
        }
    }

    public async Task<KeycloakTokenDto> ExchangeGoogleCodeForTokenAsync(string code, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post,
            $"{_options.Value.KeycloakBaseUrl}protocol/openid-connect/token");

        var body = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["client_id"] = _options.Value.PublicClientId,
            ["redirect_uri"] = _options.Value.GoogleLoginRedirectUrl
        };

        request.Content = new FormUrlEncodedContent(body);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Google auth failed: {response.StatusCode} - {error}");
        }

        var tokens = await response.Content.ReadFromJsonAsync<KeycloakTokenDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return tokens;
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var _userManagementToken = await GetAdminAccessTokenByPasswordAsync(cancellationToken);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{_options.Value.AdminBaseUrl}users/{userId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userManagementToken);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new KeyNotFoundException($"User with ID '{userId}' not found in Keycloak.");
                default:
                    throw new Exception($"Failed to delete user '{userId}' from Keycloak. Status code: {response.StatusCode}");
            }
        }
    }

    private async Task<string> GetAdminAccessTokenByPasswordAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Value.AccessTokenUrl}");

        var body = new Dictionary<string, string>
        {
            ["client_id"] = $"{_options.Value.AdminClientId}",
            ["username"] = $"{_options.Value.Username}",
            ["password"] = $"{_options.Value.Password}",
            ["grant_type"] = $"password"
        };

        request.Content = new FormUrlEncodedContent(body);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        using var doc = JsonDocument.Parse(json);
        var accessToken = doc.RootElement.GetProperty("access_token").GetString();

        return accessToken;
    }
}
