namespace LRA.Account.Application.Interfaces;

public interface IAuthService
{
    Task<bool> VerifyPasswordResetTokenAsync(string token, string email, CancellationToken cancellationToken);
    Task SendPasswordResetTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken);
    Task<bool> VerifyTwoFaTokenAsync(string token, string email, CancellationToken cancellationToken);
    Task SendTwoFactorTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken);
    Task<bool> VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
    Task SendEmailVerificationTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken);
    Task<bool> VerifyEmailChangeAsync(string token, string email, CancellationToken cancellationToken);
    Task SendEmailChangingTokenAsync(string email, int tokenLifetime, CancellationToken cancellationToken);
}
