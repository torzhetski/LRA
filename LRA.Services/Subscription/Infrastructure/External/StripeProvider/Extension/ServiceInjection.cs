using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Converter;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Services;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Stripe.Checkout;

namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddStripeServices(this IServiceCollection services)
    {
        services.AddScoped<SessionService>();
        services.AddScoped<Stripe.BillingPortal.SessionService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<IStripeService, StripeService>();
        services.AddScoped<IStripeEventConverter, StripeEventConverter>();

        return services;
    }
}
