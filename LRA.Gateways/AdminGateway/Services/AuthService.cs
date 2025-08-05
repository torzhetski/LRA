using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Gateways.AdminGateway.Services;

public class AuthService : IAuthService
{
    private readonly IAuthHttpClient _httpClient;

    public AuthService(IAuthHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task ChangeUserEmail(EmailChangeModel newEmailModel, string keycloakId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ChangeUserPasswordAsync(string newPassword, string keycloakId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ResetUserPasswordAsync(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendChangeUserEmailCodeAsync(string newEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
