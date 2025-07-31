using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;
using LRA.Subscription.Application.Interfaces;

namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Services;

public class StripeService : IStripeService
{
    private readonly SessionService _checkoutService;
    private readonly Stripe.BillingPortal.SessionService _billingPortalService;
    private readonly IOptions<StripeSettings> _options;

    public StripeService(
        SessionService sessionService,
        Stripe.BillingPortal.SessionService billingPortalService,
        IOptions<StripeSettings> options)
    {
        _checkoutService = sessionService;
        _options = options;
        _billingPortalService = billingPortalService;
        StripeConfiguration.ApiKey = _options.Value.SecretKey;
    }

    public async Task<string> CreateCheckoutSessionAsync(string customerId, string priceId, CancellationToken token)
    {
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
            Mode = "subscription",
            SuccessUrl = _options.Value.SuccessUrl,
            CancelUrl = _options.Value.CancelUrl,
            Customer = customerId,
        };

        var session = await _checkoutService.CreateAsync(options, cancellationToken: token);
        return session.Url;
    }

    public async Task<string> CreateBillingPortalSessionAsync(string customerId, CancellationToken token)
    {
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = customerId,
            ReturnUrl = _options.Value.ReturnUrl,
        };

        var session = await _billingPortalService.CreateAsync(options, cancellationToken: token);
        return session.Url;
    }
}
