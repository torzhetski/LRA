using LRA.Shared.Utils.Enums;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IVerificationRepository
{
    Task<string> CreateAsync(string email, VerificationType type, int tokenLifetime, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(string token, string email, VerificationType type, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
