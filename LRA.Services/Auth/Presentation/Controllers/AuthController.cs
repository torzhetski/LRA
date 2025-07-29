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
        var jwtToken = handler.ReadJwtToken(response.AccessToken);
        Guid.TryParse(jwtToken.Claims.First(c => c.Type == "sub").Value, out var keycloakId);

        if (await _authService.IsTwoFaEnabledAsync(keycloakId, cancellationToken))
        {
            await _authService.SendTwoFactorToken(model.Email, cancellationToken);
            var requiredVerificationDto = new VerificationDto()
            {
                IsVerificationRequired = true,
            };
            return Ok(requiredVerificationDto);
        }

        var verificationDto = new VerificationDto()
        {
            IsVerificationRequired = false,
            Tokens = response
        };
        return Ok(verificationDto);
    }

    [Authorize]
    [HttpPost("twofactor-verification")]
    public async Task<IActionResult> VerificateUser([FromBody] TwoFactorLoginModel twoFaModel, CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFaVerification(twoFaModel.TwoFactorToken, cancellationToken);

        if (response)
        {
            var tokens = await _keycklockService.LoginKeycloakUserAsync(twoFaModel, cancellationToken);
            var verificationDto = new VerificationDto()
            {
                IsVerificationRequired = false,
                Tokens = tokens,
            };
            return Ok(verificationDto);
        }
        else
        {
            return BadRequest("Invalid twofa token");
        }
    }

    [Authorize]
    [HttpPost("keycloak/refresh")]
    public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken, CancellationToken cancellationToken)
    {
        var response = await _keycklockService.RefreshTokenAsync(refreshToken, cancellationToken);
        return Ok(response);
    }
}
