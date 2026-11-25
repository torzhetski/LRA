using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Presentation.Services;

namespace LRA.Gateways.Client.Services;

public class AuthService : AuthServiceBase, IClientAuthService
{
    public AuthService(IAuthHttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetRealmConfigAsync(cancellationToken);
        return response;
    }

    public async Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserAsync(model, cancellationToken);
        return response;
    }

    public async Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken)
    {
        await _httpClient.VerifyEmailAsync(token, email, cancellationToken);
    }

    public async Task<KeycloakTokenDto> GetTokenByGoogleCode(string code, CancellationToken cancellationToken)
    {
        var response = await _httpClient.ExchangeGoogleCodeForTokenAsync(code, cancellationToken);
        return response;
    }

    public async Task<RedirectUrlModel> GetGoogleLoginUrlAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetGoogleLoginUrlAsync(cancellationToken);
        return response;
    }

    public async Task VerifyAdminResetUserPassword(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        await _httpClient.VerifyAdminResetUserPassword(resetPasswordModel, cancellationToken);
    }
}
