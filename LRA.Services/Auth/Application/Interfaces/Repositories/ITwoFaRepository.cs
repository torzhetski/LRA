using LRA.Auth.Domain.Dtos;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Test;

namespace LRA.Auth.Application.Interfaces.Repositories;

public interface ITwoFaRepository
{
    Task<TwoFaDto> CreateAsync(string email, string jwtToken, CancellationToken cancellationToken);
    Task<string> ValidateAsync(string token, Guid id, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
