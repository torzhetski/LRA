using LRA.Auth.Domain.Dtos;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces.Repositories;

public interface IVerificationRepository
{
    Task<VerificationTokenDto> CreateTwoFactorAsync(string email, string jwtToken, CancellationToken cancellationToken);
    Task<string> ValidateTwoFactorAsync(string token, CancellationToken cancellationToken);
    Task<VerificationTokenDto> CreatePasswordResetAsync(string email, CancellationToken cancellationToken);
    Task ValidatePasswordResetAsync(string token, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
