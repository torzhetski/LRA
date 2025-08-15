using LRA.Account.Application.Interfaces;
using LRA.Account.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Account.Presentation.Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    private readonly IKeycloakService _keycloakService;


    public AccountController(IAccountService testService, IKeycloakService keycloakService)
    {
        _accountService = testService;
        _keycloakService = keycloakService;
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
    public async Task<IActionResult> SoftDeleteAccount([FromRoute] Guid id, [FromQuery] Guid keycloakId, CancellationToken cancellationToken)
    {
        var deletedUser = await _accountService.SoftDeleteAccountAsync(id, keycloakId, cancellationToken);
        await _keycloakService.ChangeUserEmailAsync(deletedUser.KeycloakDeletedUserId, $"deleted.{deletedUser.Email}", cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] Guid id, [FromQuery] Guid keycloakId, CancellationToken cancellationToken)
    {
        var deletedUser = await _accountService.DeleteAccountAsync(id, keycloakId, cancellationToken);
        await _keycloakService.DeleteUserAsync (deletedUser.KeycloakDeletedUserId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("{keycloakId}")]
    public async Task<IActionResult> UpdateAccountAsync([FromBody] UpdateAccountDto dto, [FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAccountAsync(dto, keycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("twofactor/{keycloakId}")]
    public async Task<IActionResult> UpdateAccountTwoFaAsync([FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateTwoFaAsync(keycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("specified/{id}")]
    public async Task<IActionResult> UpdateSpecifiedAccountAsync([FromBody] UpdateAccountDto dto, [FromRoute] Guid id, [FromQuery] Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateSpecifiedAccountAsync(dto, id, adminKeycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("specified/twofactor/{id}")]
    public async Task<IActionResult> UpdateSpecifiedAccountTwoFaAsync([FromRoute] Guid id, [FromQuery] Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UpdateSpecifiedAccountTwoFaAsync(id, adminKeycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("filtrated")]
    public IActionResult FilterAndSearchAccounts([FromQuery] int pageNumber, [FromBody] FilterAndSearchModel model, [FromQuery] int pageSize = 10)
    {
        var response = _accountService.FilterAndSearhAccountsAsync(pageNumber, pageSize, model);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("paginated")]
    public IActionResult GetPaginated([FromQuery] int pageNumber, [FromQuery] int pageSize = 10)
    {
        var response = _accountService.GetPaginatedAccountsAsync(pageNumber, pageSize);
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

    [Authorize]
    [HttpPut("block/{id}")]
    public async Task<IActionResult> BlockAccount([FromRoute] Guid id, [FromQuery] Guid adminKeycloakId, DateTime? blockUntilUtc, CancellationToken cancellationToken)
    {
        await _accountService.BlockAccountAsync(id, adminKeycloakId, blockUntilUtc, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("unblock/{id}")]
    public async Task<IActionResult> UnblockAccount([FromRoute] Guid id, [FromQuery] Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        await _accountService.UnblockAccountAsync(id, adminKeycloakId, cancellationToken); 
        return Ok();
    }
}
