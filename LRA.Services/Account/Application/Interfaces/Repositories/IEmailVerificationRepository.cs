namespace LRA.Account.Application.Interfaces.Repositories;

public interface IEmailVerificationRepository
{
    public Task<string> CreateVerificationTokenAsync(string email, CancellationToken cancellationToken);
    public Task<bool> VerifyTokenAsync(string token, string email, CancellationToken cancellationToken);
}
