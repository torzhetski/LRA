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
using System.Reflection;

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
        var request = new HttpRequestMessage(HttpMethod.Get, $"keycloak/config");
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/registration")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        
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
        var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/login")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if(!response.IsSuccessStatusCode)
            switch (response.StatusCode) {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid user credentials");
                default: 
                    throw new Exception("smthing went wrong");
            }
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var dto = JsonSerializer.Deserialize<VerificationDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return dto;
    }

    public async Task<VerificationDto> TwoFactorVerificateAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        var request = new HttpRequestMessage(HttpMethod.Post, "twofactor-verification")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var dto = JsonSerializer.Deserialize<VerificationDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return dto;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(refreshTokenModel);
        var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/refresh")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
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
        var json = JsonSerializer.Serialize(refreshTokenModel);
        var request = new HttpRequestMessage(HttpMethod.Delete, "keycloak/logout")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

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

        var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
