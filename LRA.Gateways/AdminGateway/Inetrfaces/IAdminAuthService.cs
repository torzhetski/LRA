using LRA.Shared.Abstractions.Interfaces.Services;
using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminAuthService : IAuthServiceBase
{
    Task<TempPasswordModel> CreateBlankAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateParamedicAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateAdminAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateSuperAdminAccount(string email, CancellationToken cancellationToken);
}
