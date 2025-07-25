using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Models;
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
}
