using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;
using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models;

namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Services;

public class StripeService : IStripeService
{
    private readonly SessionService _checkoutService;
    private readonly CustomerService _customerService;
    private readonly Stripe.BillingPortal.SessionService _billingPortalService;
    private readonly IOptions<StripeSettings> _options;

    public StripeService(
        SessionService sessionService,
        Stripe.BillingPortal.SessionService billingPortalService,
        IOptions<StripeSettings> options,
        CustomerService customerService)
    {
        _checkoutService = sessionService;
        _options = options;
        _billingPortalService = billingPortalService;
        StripeConfiguration.ApiKey = _options.Value.SecretKey;
        _customerService = customerService;
    }

    public async Task<RedirectUrlModel> CreateCheckoutSessionAsync(string customerId, string priceId, CancellationToken cancellationToken)
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

        var session = await _checkoutService.CreateAsync(options, cancellationToken: cancellationToken);

        var redirectUrl = new RedirectUrlModel()
        {
            RedirectUrl = session.Url
        };

        return redirectUrl;
    }

    public async Task<RedirectUrlModel> CreateBillingPortalSessionAsync(string customerId, CancellationToken cancellationToken)
    {
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = customerId,
            ReturnUrl = _options.Value.ReturnUrl,
        };

        var session = await _billingPortalService.CreateAsync(options, cancellationToken: cancellationToken);

        var redirectUrl = new RedirectUrlModel()
        {
            RedirectUrl = session.Url
        };

        return redirectUrl;
    }

    public async Task<string> CreateStripeCustomerAsync(string email, CancellationToken cancellationToken)
    {
        var customerOptions = new CustomerCreateOptions
        {
            Email = email
        };

        var customer = await _customerService.CreateAsync(customerOptions, cancellationToken: cancellationToken);
        return customer.Id;
    }
}
