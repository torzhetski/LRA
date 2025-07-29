using LRA.Auth.Application.Interfaces;
using LRA.Auth.Presentation.Interfaces;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly IKeycklockService _keycklockService;

    public AuthController(ITestService testService, IKeycklockService keycklockService)
    {
        _testService = testService;
        _keycklockService = keycklockService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] int id)
    {
        var response = _testService.TestResponse(id);
        return Ok(response);
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
        return Ok(response);
    }

    [Authorize]
    [HttpPost("keycloak/refresh")]
    public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken, CancellationToken cancellationToken)
    {
        var response = await _keycklockService.RefreshTokenAsync(refreshToken, cancellationToken);
        return Ok(response);
    }
}
