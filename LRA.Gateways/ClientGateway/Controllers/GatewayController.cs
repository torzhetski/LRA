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

    [HttpGet("test-auth")]
    public async Task<TestMessageModel> GetAuthResponseAsync([FromQuery] int id)
    {
        return await _testService.TestAuthServiceAsync(id);
    }

    [HttpGet("test-account")]
    public async Task<TestMessageModel> GetAccountResponseAsync([FromQuery] int id)
    {
        return await _testService.TestAccountServiceAsync(id);
    }

    [HttpGet("test-subscription")]
    public async Task<TestMessageModel> GetSubscriptionResponseAsync([FromQuery] int id)
    {
        return await _testService.TestSubscriptionServiceAsync(id);
    }
}
