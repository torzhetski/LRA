using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Test;
using Microsoft.AspNetCore.Mvc;

namespace Gateways.ClientGateway.Controllers;

[Route("api/test-clients")]
[ApiController]
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

    [HttpGet("auth/{id}")]
    public async Task<IActionResult> GetAuthResponseAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _authService.TestAuthServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetAccountResponseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestAccountServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet("subscription/{id}")]
    public async Task<IActionResult> GetSubscriptionResponseAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.TestSubscriptionServiceAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost("account")]
    public async Task<IActionResult> TestPostResponseAsync([FromBody] TestDto model, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestAccountDatabaseServiceAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPost("account/brocker")]
    public async Task<IActionResult> TestBrockerAsync([FromBody] string message, CancellationToken cancellationToken)
    {
        await _accountService.TestBrokerAsync(message, cancellationToken);
        return Ok();
    }
}
