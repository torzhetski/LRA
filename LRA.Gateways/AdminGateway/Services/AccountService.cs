using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Presentation.Services;

namespace LRA.Gateways.AdminGateway.Services;

public class AccountService : AccountServiceBase, IAdminAccountService
{
    public AccountService(IAccountHttpClient accountHttpClient) : base(accountHttpClient)
    {
    }
}
