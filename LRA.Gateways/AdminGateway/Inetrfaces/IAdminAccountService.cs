using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.Services;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminAccountService : IAccountServiceBase, IRoleProvider
{
    void SomeAdminMethod();
}
