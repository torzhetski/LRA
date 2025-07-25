using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface IAuthService
{
    Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task<string> LoginUserAsync(LoginModel model, CancellationToken cancellationToken);
}
