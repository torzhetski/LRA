using LRA.Gateways.Stripe.Inerfaces;
using Microsoft.AspNetCore.Mvc;

namespace LRA.Gateways.Stripe.Controllers;

[Route("api/stripe")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly IStripeWebhookService _webhookService;

    public StripeController(IStripeWebhookService webhookService)
    {
        _webhookService = webhookService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebHookAsync(CancellationToken token)
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync(token);
        var headers = Request.Headers["Stripe-Signature"];

        await _webhookService.HandleWebHookAsync(body, headers, token);
        return Ok();
    }
}
