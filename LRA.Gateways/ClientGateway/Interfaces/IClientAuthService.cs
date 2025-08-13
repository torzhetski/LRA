using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Interfaces.Services;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Auth;


namespace LRA.Gateways.Client.Interfaces;

public interface IClientAuthService : IAuthServiceBase
{
    Task<string> GetAuthServiceConfigAsync(CancellationToken cancellationToken);
    Task<string> CreateUserAsync(RegistrationModel model, CancellationToken cancellationToken);
    Task VerifyEmailAsync(string token, string email, CancellationToken cancellationToken);
    Task<KeycloakTokenDto> GetTokenByGoogleCode(string code, CancellationToken cancellationToken);
    Task<RedirectUrlModel> GetGoogleLoginUrlAsync(CancellationToken cancellationToken);
    Task VerifyAdminResetUserPassword(ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken);
}
