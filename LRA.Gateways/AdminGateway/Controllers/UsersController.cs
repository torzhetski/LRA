using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/usres")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAdminAuthService _authService;

    public UsersController(IAdminAuthService authService)
    {
        _authService = authService;
    }

    [Authorize(Policy = ServerPolicy.RequireSuperAdmin)]
    [HttpPost("admin")]
    public async Task<IActionResult> CreateAdmin([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateAdminAccount(email, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireSuperAdmin)]
    [HttpPost("super-admin")]
    public async Task<IActionResult> CreateSuperAdmin([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateSuperAdminAccount(email, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPost("client")]
    public async Task<IActionResult> CreateClient([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateClientAccount(email, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPost("paramedic")]
    public async Task<IActionResult> CreateParamedic([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateParamedicAccount(email, cancellationToken);
        return Ok(response);
    }
}
