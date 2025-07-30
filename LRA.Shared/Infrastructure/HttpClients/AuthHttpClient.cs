using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using LRA.Shared.Utils.Exceptions;
using System.Net;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Dtos.Verification;
using Microsoft.AspNetCore.WebUtilities;
using LRA.Shared.Abstractions.Dtos;
using System.Net.Http.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class AuthHttpClient : IAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AuthHttpClient(
        HttpClient httpClient,
        IOptions<AuthServiceSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task<string> GetRealmConfigAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"keycloak/config");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        using var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/registration")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new AlreadyExistsException("User");
                default:
                    throw new Exception("smthing went wrong");
            }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        using var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/login")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid user credentials");
                default:
                    throw new Exception("smthing went wrong");
            }
        var dto = await response.Content.ReadFromJsonAsync<VerificationDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return dto;
    }

    public async Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        using var request = new HttpRequestMessage(HttpMethod.Post, "twofactor-verification")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<VerificationDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return dto;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(refreshTokenModel);
        using var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/refresh")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
                default:
                    throw new Exception("Failed to refresh token");
            }
        var token = await response.Content.ReadFromJsonAsync<KeycloakTokenDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return token;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(refreshTokenModel);
        using var request = new HttpRequestMessage(HttpMethod.Delete, "keycloak/logout")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
                default:
                    throw new Exception("Failed to logout");
            }
    }

    public async Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("change-password", new Dictionary<string, string?>
        {
            ["newPassword"] = newPassword,
            ["keycloakId"] = keycloakId.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task ChangeUserEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("change-email", new Dictionary<string, string?>
        {
            ["newEmail"] = newEmail,
            ["keycloakId"] = keycloakId.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
