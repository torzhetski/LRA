using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace LRA.Subscription.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IStripeService _stripeService;
    private readonly ICustomerProviderService _customerService;
    private readonly IOptions<PriceSettings> _priceOptions;

    public PaymentService(
        IStripeService service,
        IOptions<PriceSettings> priceOptions,
        ICustomerProviderService customerService)
    {
        _stripeService = service;
        _priceOptions = priceOptions;
        _customerService = customerService;
    }

    public async Task<RedirectUrlModel> GetCheckoutSessionUrlAsync(Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var customerId = await _customerService.GetCustomerId(keycloakId, email, cancellationToken);
        var url = await _stripeService.CreateCheckoutSessionAsync(customerId, _priceOptions.Value.Id, cancellationToken);
        return url;
    }

    public async Task<RedirectUrlModel> GetCustomersPortalUrlAsync(Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var customerId = await _customerService.GetCustomerId(keycloakId, email, cancellationToken);
        var url = await _stripeService.CreateBillingPortalSessionAsync(customerId, cancellationToken);
        return url;
    }
}
