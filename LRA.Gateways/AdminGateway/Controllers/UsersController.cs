using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginatedAccounts([FromQuery] int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetPaginatedAccountsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPost("filtrated")]
    public async Task<IActionResult> FilterAndSearchAccounts([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromBody] FilterAndSearchModel model, CancellationToken cancellationToken)
    {
        var response = await _accountService.FilterAndSearhAccountsAsync(pageNumber, pageSize, model, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountById(id, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDeleteAccount([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.SoftDeleteAsync(id, userId, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireSuperAdmin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.DeleteAsync(id, userId, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("block/{id}")]
    public async Task<IActionResult> BlockAccount([FromRoute] Guid id, [FromQuery] DateTime? blockUntilUtc, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.BlockAccount(id, userId, blockUntilUtc, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("unblock/{id}")]
    public async Task<IActionResult> UnblockAccount([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.UnblockAccount(id, userId, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("specified/{id}")]
    public async Task<IActionResult> UpdateSpecifiedAccountAsync([FromBody] UpdateAccountDto dto, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.UpdateSpecifiedAccountAsync(dto, id, userId, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("specified/twofactor/{id}")]
    public async Task<IActionResult> UpdateSpecifiedAccountTwoFaAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.UpdateSpecifiedAccountTwoFaAsync(id, userId, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("temp-password/{id}")]
    public async Task<IActionResult> GenerateTempPassword([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        var passModel = await _authService.GenerateTempPassword(id, userId, cancellationToken);

        return Ok(passModel);
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet("admin-reset-code")]
    public async Task<IActionResult> SendAdminResetUserPasswordCode([FromQuery] string email, CancellationToken cancellationToken)
    {
        await _authService.SendResetPasswordCodeAsync(email, cancellationToken);
        return Ok();
    }
}
