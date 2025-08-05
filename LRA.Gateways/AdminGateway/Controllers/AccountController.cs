using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/admin")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpPut("account")]
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

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
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
