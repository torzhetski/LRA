using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [Authorize]
    [HttpPost("auth/change-password")]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromQuery] string newPassword, CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null || !Guid.TryParse(userIdStr, out var userId)) 
        {
            return Unauthorized("Missing 'sub' in token");
        }

        await _authService.ChangeUserPasswordAsync(newPassword, userId, cancellationToken);
        return Ok();
    }

    [HttpPost("twofactor-verification")]
    public async Task<IActionResult> VerificateUser([FromQuery] string token, CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFactorVerificateAsync(token, cancellationToken);
        return Ok(response);
    }
}
