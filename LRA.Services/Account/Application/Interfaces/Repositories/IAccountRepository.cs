using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Guid> CreateAsync(AccountEntity model, CancellationToken cancellationToken);
    Task<AccountDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountDto> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<AccountDto> GetByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    PaginatedList<AccountDto> GetPaginatedData(int pageNumber, int pageSize);
    Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken);
    void SoftDelete (Guid id);
    Task SoftDeleteAsync (Guid id, CancellationToken cancellationToken);
    void Delete(Guid id);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    void Update(UpdateAccountDto dto, Guid keycloakId);
    Task UpdateAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken);
    Task MakeUserVerifiedAsync (string email, CancellationToken cancellationToken);
    Task<bool> IsAccountExists(Guid keycloakId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
