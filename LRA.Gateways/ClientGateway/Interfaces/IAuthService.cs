using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAuthService
{
    Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerificateAsync(TwoFactorLoginModel model, CancellationToken cancellationToken);
}
