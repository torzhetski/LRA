using LRA.Account.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LRA.Shared.Abstractions.Dtos;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Account.Domain.Entities;

namespace AccountService.Presentation.Controllers;

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
        await _accountService.SoftDeleteAccount(id, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateAsync([FromBody] BaseAccountDto dto, CancellationToken cancellationToken)
    {
        var model = new AccountEntity()
        {
            KeycloakId = dto.KeycloakId,
            Email = dto.Email,
        };
        var response = await _accountService.CreateAccountAsync(model, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateAsync([FromBody] AccountDto dto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAccountAsync(dto, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("update-twofa")]
    public async Task<IActionResult> UpdateTwoFaAsync([FromQuery] Guid keycloakId, CancellationToken cancellationToken)
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

    [Authorize]
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyAsync([FromQuery] string token, [FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await _accountService.VerifyEmailAsync(token, email, cancellationToken);
        if (result)
            return Ok(new GenericServiceResponse<string>("email verified"));
        else
            return BadRequest(new GenericServiceResponse<string>("email not verified"));
    }

    [Authorize]
    [HttpGet("two-fa/{keycloakId}")]
    public async Task<IActionResult> GetAll([FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        var data = await _accountService.IsTwoFaEnabledAsync(keycloakId, cancellationToken);

        var response = new GenericServiceResponse<bool>(data);
        return Ok(response);
    }
}
