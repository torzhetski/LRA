using LRA.Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Dtos.Account;

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
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("keycloak/{keycloakId}")]
    public async Task<IActionResult> GetAccountByKeycloakId([FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountByKeycloakIdAsync(keycloakId, cancellationToken);
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
    [HttpPut("{keycloakId}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateAccountDto dto, [FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAccountAsync(dto, keycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("twofactor/{keycloakId}")]
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
    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAllAccountsAsync(cancellationToken);
        return Ok(response);
    }

    [HttpGet("{keycloakId}/roles")]
    public async Task<IActionResult> GetRolesByKeycloakId([FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        var roles = await _accountService.GetRolesByKeycloakIdAsync(keycloakId, cancellationToken);

        return Ok(roles);
    }
}
