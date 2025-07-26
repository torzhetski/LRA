using LRA.Auth.Application.Interfaces;
using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Presentation.Controllers;

[Route("api/test-auth")]
public class TestController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IKeycklockService _keycklockService;

    public TestController(IAuthService authService, IKeycklockService keycklockService)
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
            var tokenId = await _authService.SendTwoFactorToken(model.Email, response, cancellationToken);
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
    public async Task<IActionResult> VerificateUser([FromQuery] string token, Guid id ,CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFaVerification(token, id, cancellationToken);

        var verificationDto = new VerificationDto()
        {
            IsVerificationRequired = false,
            Token = response,
        };

        return Ok(verificationDto);
    }
}
