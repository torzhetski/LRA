using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Interfaces.Services;

namespace LRA.Shared.Presentation.Services;

public abstract class AuthServiceBase : IAuthServiceBase
{
    protected readonly IAuthHttpClient _httpClient;

    public AuthServiceBase(IAuthHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.LoginUserAsync(model, cancellationToken);
        return response;
    }

    public async Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var response = await _httpClient.TwoFactorVerifyAsync(model, cancellationToken);
        return response;
    }

    public async Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _httpClient.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return response;
    }

    public async Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _httpClient.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
    }

    public async Task ChangeUserPasswordAsync(string newPassword, string keycloakId, CancellationToken cancellationToken)
    {
        await _httpClient.ChangeUserPasswordAsync(newPassword, keycloakId, cancellationToken);
    }

    public async Task SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken)
    {
        await _httpClient.SendResetPasswordCodeAsync(email, cancellationToken);
    }

    public async Task ResetUserPasswordAsync(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        await _httpClient.ResetPasswordVerifyAsync(resetPasswordModel, cancellationToken);
    }

    public async Task SendChangeUserEmailCodeAsync(string newEmail, CancellationToken cancellationToken)
    {
        await _httpClient.SendChangeUserEmailCodeAsync(newEmail, cancellationToken);
    }

    public async Task ChangeUserEmail(EmailChangeModel newEmailModel, string keycloakId, CancellationToken cancellationToken)
    {
        await _httpClient.ChangeUserEmailVerifyAsync(newEmailModel, keycloakId, cancellationToken);
    }
}
