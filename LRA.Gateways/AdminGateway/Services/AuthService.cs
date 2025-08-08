using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Presentation.Services;
using LRA.Shared.Utils.Constants;

namespace LRA.Gateways.AdminGateway.Services;

public class AuthService : AuthServiceBase, IAdminAuthService
{
    public AuthService(IAuthHttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<TempPasswordModel> CreateAdminAccount(string email, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserWithTempPassword(email, SystemRoles.Admin, cancellationToken);
        return response;
    }

    public async Task<TempPasswordModel> CreateBlankAccount(string email, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserWithTempPassword(email, null, cancellationToken);
        return response;
    }

    public async Task<TempPasswordModel> CreateParamedicAccount(string email, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserWithTempPassword(email, SystemRoles.Paramedic, cancellationToken);
        return response;
    }

    public async Task<TempPasswordModel> CreateSuperAdminAccount(string email, CancellationToken cancellationToken)
    {
        var response = await _httpClient.CreateUserWithTempPassword(email, SystemRoles.SuperAdmin, cancellationToken);
        return response;
    }
}
