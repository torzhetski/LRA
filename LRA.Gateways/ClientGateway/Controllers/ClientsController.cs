using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using Microsoft.AspNetCore.Mvc;

namespace Gateways.ClientGateway.Controllers;

[Route("api/v{version:apiVersion}/test-clients")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ClientsController : ControllerBase
{
    private readonly ITestAccountService _accountService;
    private readonly ITestAuthService _authService;
    private readonly ITestSubscriptionService _subscriptionService;

    public ClientsController(
        ITestAccountService testService, 
        ITestAuthService authService, 
        ITestSubscriptionService subscriptionService)
    {
        _accountService = testService;
        _authService = authService;
        _subscriptionService = subscriptionService;
    }

    [MapToApiVersion("2.0")]
    [HttpGet("auth/config")]
    public async Task<IActionResult> GetAuthConfigAsync(CancellationToken cancellationToken)
    {
        var response = await _authService.GetAuthServiceConfigAsync(cancellationToken);
        return Ok(response);
    }

    [MapToApiVersion("2.0")]
    [HttpPost("auth")]
    public async Task<IActionResult> CreateUserAsync([FromBody] RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [MapToApiVersion("2.0")]
    [HttpGet("subscription/{id}")]
    public async Task<IActionResult> GetSubscriptionResponseAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.TestSubscriptionServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetAccountResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestGetFromAccountServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet("account/paginated")]
    public async Task<IActionResult> GetPaginatedAccountsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestGetPaginatedFromAccountServiceAsync(pageNumber, pageSize, cancellationToken);
        return Ok(response);
    }

    [HttpGet("account")]
    public async Task<IActionResult> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _accountService.TestGetAllfromAccountServiceAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost("account")]
    public async Task<IActionResult> TestPostResponseAsync([FromBody] TestDto model, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestAccountDatabaseServiceAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("account/soft/{id}")]
    public async Task<IActionResult> TestSoftDeleteResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _accountService.TestAccountSoftDelete(id, cancellationToken);
        return Ok();
    }

    [HttpPost("account/brocker")]
    public async Task<IActionResult> SendEmailAsync([FromBody] EmailMessageModel messageModel, CancellationToken cancellationToken)
    {
        await _accountService.SendEmailMessageAsync(messageModel, cancellationToken);
        return Ok();
    }
}
