using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAccountService
{
    Task<AccountDto> TestGetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<AccountDto>> TestGetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountDto>> TestGetAllfromAccountServiceAsync(CancellationToken cancellationToken);
    Task<TestMessageModel> TestAccountDatabaseServiceAsync(AccountDto model, CancellationToken cancellationToken);
    Task SendEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken);
    Task TestAccountSoftDelete(Guid id, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
}
