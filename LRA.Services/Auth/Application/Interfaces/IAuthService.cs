using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces;

public interface IAuthService
{
    Task<bool> IsTwoFaEnabledAsync (Guid keycloakId, CancellationToken cancellationToken);
    Task<string> TwoFaVerification(string token, Guid id, CancellationToken cancellationToken);
    Task<Guid> SendTwoFactorToken(string email, string jwtToken, CancellationToken cancellationToken);
}
