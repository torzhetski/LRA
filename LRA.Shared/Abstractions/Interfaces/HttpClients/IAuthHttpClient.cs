using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Dtos;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAuthHttpClient
{
    Task<string> GetRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerifyAsync(TwoFactorLoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, Guid keycloakId, CancellationToken cancellationToken);
    Task ChangeUserEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
}
