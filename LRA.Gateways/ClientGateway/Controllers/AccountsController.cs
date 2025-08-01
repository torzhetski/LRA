using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gateways.ClientGateway.Controllers;

[Route("api/v{version:apiVersion}/clients")]
[ApiController]
[ApiVersion("1.0")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize]
    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetAccountResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetFromAccountServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("account/paginated")]
    public async Task<IActionResult> GetPaginatedAccountsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetPaginatedFromAccountServiceAsync(pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAllFromAccountServiceAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireClient)]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.UpdateAccountAsync(dto, userId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("twofa/update")]
    public async Task<IActionResult> UpdateAccountTwoFa(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _accountService.UpdateAccountTwoFa(userId, cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete("account/soft/{id}")]
    public async Task<IActionResult> SoftDeleteResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _accountService.AccountSoftDelete(id, cancellationToken);
        return Ok();
    }
}
