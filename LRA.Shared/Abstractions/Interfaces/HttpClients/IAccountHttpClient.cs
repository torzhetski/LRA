using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<AccountDto> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<AccountDto>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TestMessageModel> PostAsync(AccountDto model, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task PostEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken);
    Task VerifyEmailAsync (string token, string email, CancellationToken cancellationToken);
}
