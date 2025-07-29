namespace LRA.Auth.Application.Interfaces.Repositories;

public interface ITwoFaRepository
{
    Task<string> CreateAsync(string email, CancellationToken cancellationToken);
    Task<bool> ValidateAsync(string token, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
