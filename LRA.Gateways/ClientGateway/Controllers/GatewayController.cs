using LRA.Gateways.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Interfaces;
using Shared.Abstractions.Test;

namespace Gateways.ClientGateway.Controllers;

[Route("api/clients")]
[ApiController]
public class GatewayController : ControllerBase
{
    private readonly ITestService _testService;

    public GatewayController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet("auth")]
    public async Task<TestMessageModel> GetAuthResponseAsync([FromQuery] int id)
    {
        return await _testService.TestAuthServiceAsync(id);
    }

    [HttpGet("account")]
    public async Task<TestMessageModel> GetAccountResponseAsync([FromQuery] int id)
    {
        return await _testService.TestAccountServiceAsync(id);
    }

    [HttpGet("subscription")]
    public async Task<TestMessageModel> GetSubscriptionResponseAsync([FromQuery] int id)
    {
        return await _testService.TestSubscriptionServiceAsync(id);
    }
}
