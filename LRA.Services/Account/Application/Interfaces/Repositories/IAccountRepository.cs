using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Application.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Guid> CreateAsync(AccountEntity model, CancellationToken cancellationToken);
    Task<AccountDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AccountDto> GetByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken);
    void SoftDelete (Guid id);
    Task SoftDeleteAsync (Guid id, CancellationToken cancellationToken);
    void Delete(Guid id);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    void Update(AccountDto dto);
    Task UpdateAsync(AccountDto dto, CancellationToken cancellationToken);
    Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken);
    PaginatedList<AccountDto> GetPaginatedData(int pageNumber, int pageSize);
    Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken);
    Task MakeUserVerifiedAsync (string email, CancellationToken cancellationToken);   
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
