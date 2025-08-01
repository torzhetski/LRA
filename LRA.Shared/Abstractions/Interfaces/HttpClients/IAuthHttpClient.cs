using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Dtos;
using System.Dynamic;
using LRA.Shared.Abstractions.Models;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAuthHttpClient
{
    Task<string> GetRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken);
    Task VerifyEmailAsync (string token, string email, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, string keycloakId, CancellationToken cancellationToken);
    Task SendResetPasswordCodeAsync(string email, CancellationToken cancellationToken);
    Task ResetPasswordVerifyAsync(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken);
    Task SendChangeUserEmailCodeAsync(string newEmail, CancellationToken cancellationToken);
    Task ChangeUserEmailVerifyAsync(EmailChangeModel newEmailModel, string keycloakId, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> ExchangeGoogleCodeForTokenAsync(string code, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetGoogleLoginUrlAsync(CancellationToken cancellationToken);
}
