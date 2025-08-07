using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Application.Services;
using LRA.Subscription.Application.Services.Hosted;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Subscription.Application.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ICustomerProviderService, CustomerProviderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddHostedService<CustomerRabbitMQConsumer>();
        services.AddHostedService<WebhookRabbitMQConsumer>();

        return services;
    }
}
