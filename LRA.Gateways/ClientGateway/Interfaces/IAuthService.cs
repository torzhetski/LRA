using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.Client.Interfaces;

public interface IAuthService
{
    Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, string keycloakId, CancellationToken cancellationToken);
    Task SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken);
    Task ResetUserPasswordAsync(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken);
    Task SendChangeUserEmailCodeAsync(string newEmail, CancellationToken cancellationToken);
    Task ChangeUserEmail(EmailChangeModel newEmailModel, string keycloakId, CancellationToken cancellationToken);
}
