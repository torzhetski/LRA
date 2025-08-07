using LRA.Shared.Abstractions.Models;
using LRA.Shared.Utils.Exceptions;
using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LRA.Subscription.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IStripeService _stripeService;
    private readonly ICustomerProviderService _customerService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IOptions<PriceSettings> _priceOptions;

    public PaymentService(
        IStripeService service,
        IOptions<PriceSettings> priceOptions,
        ICustomerProviderService customerService,
        ISubscriptionService subscriptionService)
    {
        _stripeService = service;
        _priceOptions = priceOptions;
        _customerService = customerService;
        _subscriptionService = subscriptionService;
    }

    public async Task<RedirectUrlModel> GetCheckoutSessionUrlAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var customerId = await _customerService.GetCustomerId(keycloakId, cancellationToken);

        var hasActive = await _subscriptionService.HasCustomerActiveSubscription(customerId, cancellationToken);
        if (hasActive) 
        { 
            throw new AlreadyExistsException("Subscription");    
        }
        else
        {
            var url = await _stripeService.CreateCheckoutSessionAsync(customerId, _priceOptions.Value.Id, cancellationToken);
            return url;
        }
    }

    public async Task<RedirectUrlModel> GetCustomersPortalUrlAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var customerId = await _customerService.GetCustomerId(keycloakId, cancellationToken);
        var url = await _stripeService.CreateBillingPortalSessionAsync(customerId, cancellationToken);
        return url;
    }
}
