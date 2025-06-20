using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstructions.Test;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Test;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text;

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

    [HttpGet("auth")]
    public async Task<TestMessageModel> GetAuthResponseAsync([FromQuery] int id, CancellationToken cancellationToken)
    {
        var response = await _authService.TestAuthServiceAsync(id, cancellationToken);
        return response;
    }

    [HttpGet("account")]
    public async Task<TestMessageModel> GetAccountResponseAsync([FromQuery] int id, CancellationToken cancellationToken)
    {
        var response = await _accountService.TestAccountServiceAsync(id, cancellationToken);
        return response;
    }

    [HttpGet("subscription")]
    public async Task<TestMessageModel> GetSubscriptionResponseAsync([FromQuery] int id, CancellationToken cancellationToken)
    {
        var response = await _subscriptionService.TestSubscriptionServiceAsync(id, cancellationToken);
        return response;
    }

    //[HttpPost("account")]
    //public async Task<TestMessageModel> CreateAccountAsync([FromBody] TestModel model)
    //{
    //    //var json = JsonSerializer.Serialize(model);
    //    //var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7003/api/test-account")
    //    //{
    //    //    Content = new StringContent(json, Encoding.UTF8, "application/json")
    //    //};
    //    //return await _http.SendAsync<TestMessageModel>(request);
    //}
}
