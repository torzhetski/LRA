namespace LRA.Auth.Application.Interfaces;

public interface IAuthService
{
    Task<bool> IsTwoFaEnabledAsync (Guid keycloakId, CancellationToken cancellationToken);
    Task PasswordResetVerificationAsync(string token, CancellationToken cancellationToken);
    Task SendPasswordResetTokenAsync(string email, CancellationToken cancellationToken);
    Task<bool> TwoFaVerification(string token, CancellationToken cancellationToken);
    Task SendTwoFactorToken(string email, CancellationToken cancellationToken);
}
