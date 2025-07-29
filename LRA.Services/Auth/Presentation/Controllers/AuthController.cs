using LRA.Auth.Application.Interfaces;
using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Presentation.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IKeycklockService _keycklockService;

    public AuthController(IAuthService authService, IKeycklockService keycklockService)
    {
        _authService = authService;
        _keycklockService = keycklockService;
    }

    [Authorize]
    [HttpGet("keycloak/config")]
    public async Task<IActionResult> GetRealmConfig(CancellationToken cancellationToken)
    {
        var response = await _keycklockService.GetKeycloakRealmConfigAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("keycloak/registration")]
    public async Task<IActionResult> CreateUser([FromBody] RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _keycklockService.CreateKeycloakUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("keycloak/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _keycklockService.LoginKeycloakUserAsync(model, cancellationToken);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(response);
        Guid.TryParse(jwtToken.Claims.First(c => c.Type == "sub").Value, out var keycloakId);

        if (await _authService.IsTwoFaEnabledAsync(keycloakId, cancellationToken))
        {
            var tokenId = await _authService.SendTwoFactorTokenAsync(model.Email, response, cancellationToken);
            var requiredVerificationDto = new VerificationDto()
            {
                IsVerificationRequired = true,
                VerificationId = tokenId.ToString(),
            };
            return Ok(requiredVerificationDto);
        }

        var verificationDto = new VerificationDto()
        {
            IsVerificationRequired = false,
            Token = response,
        };
        return Ok(verificationDto);
    }

    [Authorize]
    [HttpPost("twofactor-verification")]
    public async Task<IActionResult> VerificateUser([FromQuery] string token, CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFaVerificationAsync(token, cancellationToken);

        var verificationDto = new VerificationDto() 
        {
            IsVerificationRequired = false,
            Token = response,
        };

        return Ok(verificationDto);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangeUserPassword([FromQuery] string newPassword, [FromQuery] Guid id ,CancellationToken cancellationToken)
    {
        await _keycklockService.ChangeUserPasswordAsync(newPassword, id, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("reset-password/code")]
    public async Task<IActionResult> ResetUserPassword([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.SendPasswordResetTokenAsync(email, cancellationToken);
        return Ok(new GenericServiceResponse<string>(response.ToString()));
    }

    [Authorize]
    [HttpPost("reset-password/verification")]
    public async Task<IActionResult> ResetUserPassword([FromQuery] string newPassword, string email, string token, CancellationToken cancellationToken)
    {
        await _authService.PasswordResetVerificationAsync(token, cancellationToken);
        //вот тут надо получить айдишник по мейлу - либо я подниму таблицу в этом сервисе, либо обращаться к аккаунту чтоб получить айдишник
        //дальше просто вызываем тот же await _keycklockService.ChangeUserPasswordAsync(newPassword, id, cancellationToken); что и в обычной смене пароля
        return Ok();
    }
}
