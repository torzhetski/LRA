using LRA.Subscription.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LRA.Subscription.Presentation.Controllers;

[Route("api/subscription")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private ISubscriptionService _subscriptionService;

    public SubscriptionController(ISubscriptionService testService)
    {
        _subscriptionService = testService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInfo([FromRoute] int id)
    {
        var response = _subscriptionService.TestResponse(id);
        return Ok(response);
    }
}
