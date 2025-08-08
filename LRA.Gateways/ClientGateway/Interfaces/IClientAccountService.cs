using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.Services;

namespace LRA.Gateways.Client.Interfaces;

public interface IClientAccountService : IAccountServiceBase, IRoleProvider
{
}
