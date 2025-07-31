using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Subscription.Application.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ICustomerProviderService, CustomerProviderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
