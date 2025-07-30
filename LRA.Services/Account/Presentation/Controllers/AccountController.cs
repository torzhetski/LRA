using LRA.Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Account.Domain.Entities;

namespace LRA.Account.Presentation.Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;

    public AccountController(IAccountService testService)
    {
        _accountService = testService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDeleteTest([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _accountService.SoftDeleteAccountAsync(id, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateAsync([FromBody] AccountDto dto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAccountAsync(dto, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("update-twofa/{keycloakId}")]
    public async Task<IActionResult> UpdateTwoFaAsync([FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateTwoFaAsync(keycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("paginated")]
    public IActionResult GetPaginated([FromQuery] int pageNumber, [FromQuery] int PageSize)
    {
        var response = _accountService.GetPaginatedAccounts(pageNumber, PageSize);
        return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(response);
    }
}
