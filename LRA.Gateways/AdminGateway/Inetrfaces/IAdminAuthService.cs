using LRA.Shared.Abstractions.Interfaces.Services;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Gateways.AdminGateway.Inetrfaces;

public interface IAdminAuthService : IAuthServiceBase
{
    Task<TempPasswordModel> CreateClientAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateParamedicAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateAdminAccount(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> CreateSuperAdminAccount(string email, CancellationToken cancellationToken);
    Task SendAdminResetUserPasswordCode(string email, CancellationToken cancellationToken);
    Task<TempPasswordModel> GenerateTempPassword(Guid id, string adminKeyclaokId, CancellationToken cancellationToken);
}
