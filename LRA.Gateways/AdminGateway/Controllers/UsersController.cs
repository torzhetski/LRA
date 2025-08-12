using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/usres")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAdminAuthService _authService;
    private readonly IAdminAccountService _accountService;

    public UsersController(IAdminAuthService authService, IAdminAccountService accountService)
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

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetAllAccounts( CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet]
    public async Task<IActionResult> GetPaginatedAccounts([FromQuery] int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetPaginatedAccountsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPost("filtrated")]
    public IActionResult FilterAndSearchAccounts([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromBody] FilterAndSearchModel model, CancellationToken cancellationToken)
    {
        var response = _accountService.FilterAndSearhAccountsAsync(pageNumber, pageSize, model, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountById(id, cancellationToken);
        return Ok(response);
    }
}
