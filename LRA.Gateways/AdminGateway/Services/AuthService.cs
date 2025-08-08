using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Presentation.Services;

namespace LRA.Gateways.AdminGateway.Services;

public class AuthService : AuthServiceBase, IAdminAuthService
{
    public AuthService(IAuthHttpClient httpClient) : base(httpClient)
    {
    }
}
