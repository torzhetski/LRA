using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAuthHttpClient
{
    Task<string> GetRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerificateAsync(TwoFactorLoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
    Task LogoutUserSessionAsync(RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken);
}
