using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gateways.ClientGateway.Controllers;

[Route("api/v{version:apiVersion}/account")]
[ApiController]
[ApiVersion("1.0")]
public class AccountsController : ControllerBase
{
    private readonly IClientAccountService _accountService;

    public AccountsController(IClientAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAccountResponseAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        var response = await _accountService.GetAccountAsync(userId, cancellationToken);
        return Ok(response);
    }

    [Authorize(Policy = ServerPolicy.RequireClient)]
    [HttpPut]
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
    [HttpPut("twofactor")]
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
}
