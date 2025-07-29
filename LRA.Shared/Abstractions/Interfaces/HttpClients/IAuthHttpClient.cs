using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Abstractions.Test;
using System.Threading;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAuthHttpClient
{
    Task<string> GetRealmConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<VerificationDto> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
    Task ChangeUserPasswordAsync(string newPassword, Guid id, CancellationToken cancellationToken);
    Task ChangeUserEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
    Task<VerificationDto> TwoFactorVerificateAsync(string token, CancellationToken cancellationToken);
}
