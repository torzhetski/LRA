using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace LRA.Shared.Infrastructure.HttpClients;

public class AuthHttpClient : IAuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AuthHttpClient(
        HttpClient httpClient,
        IOptions<AuthEndpointsSettings> options)
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
        using var request = new HttpRequestMessage(HttpMethod.Post, "account/registration")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new AlreadyExistsException("Account");
                default:
                    throw new Exception("Account creation failed");
            }
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public async Task<TempPasswordModel> CreateUserWithTempPassword(string email, string role, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("account-with-temp-pass/registration", new Dictionary<string, string?>
        {
            ["email"] = email,
            ["role"] = role,
        });
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        var password = await response.Content.ReadFromJsonAsync<TempPasswordModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        return password;
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
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid account credentials");
                case HttpStatusCode.Locked:
                    throw new UserBlockedException(model.Email);
                case HttpStatusCode.BadRequest:
                    throw new InvalidVerificationException();
                default:
                    throw new Exception("Account login failed");
            }
        }

        var dto = await response.Content.ReadFromJsonAsync<VerificationDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return dto;
    }

    public async Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(model);
        using var request = new HttpRequestMessage(HttpMethod.Post, "twofactor/verification")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new InvalidParametrException();
                default:
                    throw new Exception("TwoFactor verification failed");
            }
        }

        var dto = await response.Content.ReadFromJsonAsync<VerificationDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return dto;
    }

    public async Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("email/verification", new Dictionary<string, string?>
        {
            ["token"] = token,
            ["email"] = email
        });

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new InvalidParametrException();
                default:
                    throw new Exception("Email verification failed");
            }
        }
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(refreshTokenModel);
        using var request = new HttpRequestMessage(HttpMethod.Post, "keycloak/refreshment")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
                default:
                    throw new Exception("Failed to refresh token");
            }
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
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Invalid or expired refresh token");
                default:
                    throw new Exception("Failed to logout");
            }
        }
    }

    public async Task ChangeUserPasswordAsync(string newPassword, string keycloakId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString($"new-password/{keycloakId}", new Dictionary<string, string?>
        {
            ["newPassword"] = newPassword,
        });

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("reset-code", new Dictionary<string, string?>
        {
            ["email"] = email,
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task ResetPasswordVerifyAsync(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(resetPasswordModel);
        using var request = new HttpRequestMessage(HttpMethod.Put, "reset-code/verification")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new InvalidParametrException();
                default:
                    throw new Exception("Reset password verification failed");
            }
        }
    }

    public async Task SendChangeUserEmailCodeAsync(string newEmail, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("new-email-code", new Dictionary<string, string?>
        {
            ["newEmail"] = newEmail,
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task ChangeUserEmailVerifyAsync(EmailChangeModel newEmailModel, string keycloakId, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(newEmailModel);

        using var request = new HttpRequestMessage(HttpMethod.Put, $"new-email-code/verification/{keycloakId}")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<KeycloakTokenDto> ExchangeGoogleCodeForTokenAsync(string code, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("keycloak/google-login", new Dictionary<string, string?>
        {
            ["code"] = code,
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<KeycloakTokenDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return token;
    }

    public async Task<RedirectUrlModel> GetGoogleLoginUrlAsync(CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "google-url");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var url = await response.Content.ReadFromJsonAsync<RedirectUrlModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);

        return url;
    }

    public async Task SendAdminResetUserPasswordCode(string email, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("admin-reset-code", new Dictionary<string, string?>
        {
            ["email"] = email,
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task VerifyAdminResetUserPassword(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(resetPasswordModel);
        using var request = new HttpRequestMessage(HttpMethod.Put, "admin-reset-code/verification")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new InvalidParametrException();
                default:
                    throw new Exception("Reset password verification failed");
            }
        }
    }

    public async Task<TempPasswordModel> GenerateTempPassword(Guid id, string adminKeyclaokId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString($"temp-password/{id}", new Dictionary<string, string?>
        {
            ["adminKeyclaokId"] = adminKeyclaokId,
        });
        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        var password = await response.Content.ReadFromJsonAsync<TempPasswordModel>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        return password;
    }
}
