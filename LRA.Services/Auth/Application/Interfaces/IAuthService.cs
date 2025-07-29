using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces;

public interface IAuthService
{
    Task<bool> IsTwoFaEnabledAsync (Guid keycloakId, CancellationToken cancellationToken);
    Task<string> TwoFaVerificationAsync(string token, CancellationToken cancellationToken);
    Task<Guid> SendTwoFactorTokenAsync(string email, string jwtToken, CancellationToken cancellationToken);
    Task PasswordResetVerificationAsync(string token, CancellationToken cancellationToken);
    Task<Guid> SendPasswordResetTokenAsync(string email, CancellationToken cancellationToken);
}
