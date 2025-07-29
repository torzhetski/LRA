using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAuthService
{
    Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync (string newPassword, Guid keycloakId, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerificateAsync(string token, CancellationToken cancellationToken);
}
