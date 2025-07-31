using Microsoft.Extensions.DependencyInjection;

namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddStripeServices(this IServiceCollection services)
    {
        //services.AddScoped<SessionService>();
        //services.AddScoped<Stripe.BillingPortal.SessionService>();
        //services.AddScoped<IStripeService, StripeService>();
        //services.AddScoped<IStripeEventConverter, StripeEventConverter>();

        return services;
    }
}