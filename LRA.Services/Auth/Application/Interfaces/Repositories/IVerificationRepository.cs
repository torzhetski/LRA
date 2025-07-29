namespace LRA.Auth.Application.Interfaces.Repositories;

public interface IVerificationRepository
{
    Task<string> CreateAsync(string email, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(string token, CancellationToken cancellationToken);
    Task<string> CreatePasswordResetAsync(string email, CancellationToken cancellationToken);
    Task ValidatePasswordResetAsync(string token, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
