using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Presentation.Services;

namespace LRA.Gateways.Client.Services;

public class AccountService : AccountServiceBase, IClientAccountService
{
    public AccountService(IAccountHttpClient accountHttpClient) : base(accountHttpClient)
    {
    }
}
