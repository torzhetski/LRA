using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateways.ClientGateway.Controllers;

[Route("api/v{version:apiVersion}/test-clients")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
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
    [HttpPost("account/verify-email")]
    public async Task<IActionResult> VerifyEmailAsync([FromQuery] string token, [FromQuery] string email, CancellationToken cancellationToken)
    {
        await _accountService.VerifyEmailAsync(token, email, cancellationToken);
        return Ok();
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
        var response = await _accountService.GetAllfromAccountServiceAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("account/soft/{id}")]
    public async Task<IActionResult> TestSoftDeleteResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _accountService.AccountSoftDelete(id, cancellationToken);
        return Ok();
    }
}
