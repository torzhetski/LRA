using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.Client.Controllers;

[Route("api/v{version:apiVersion}/test-clients")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [Authorize]
    [HttpGet("auth/config")]
    public async Task<IActionResult> GetAuthConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _authService.GetAuthServiceConfigAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost("auth/registration")]
    public async Task<IActionResult> CreateUserAsync([FromBody] RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPost("twofactor-verification")]
    public async Task<IActionResult> VerificateUser([FromBody] TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFactorVerificateAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPost("auth/refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("keycloak/logout")]
    public async Task<IActionResult> LogoutSession([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _authService.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
        return Ok();
    }
}
