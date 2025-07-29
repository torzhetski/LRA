using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetFromAccountServiceAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedList<AccountDto>> GetPaginatedFromAccountServiceAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<AccountDto>> GetAllfromAccountServiceAsync(CancellationToken cancellationToken);
    Task SendEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken);
    Task AccountSoftDelete(Guid id, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
}
