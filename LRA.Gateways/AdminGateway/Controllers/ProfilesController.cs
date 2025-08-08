using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/profile")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly IAdminAuthService _authService;
    private readonly IAdminAccountService _accountService;

    public ProfilesController(IAdminAuthService authService, IAdminAccountService accountService)
    {
        _authService = authService;
        _accountService = accountService;
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
    [HttpPost("blank")]
    public async Task<IActionResult> CreateBlank([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateBlankAccount(email, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPost("paramedic")]
    public async Task<IActionResult> CreateParamedic([FromQuery] string email, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateParamedicAccount(email, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetAllAccounts( CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet]
    public async Task<IActionResult> CreateParamedic([FromQuery] int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetPaginatedAccountsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }
}
