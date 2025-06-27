using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Services;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace LRA.Gateways.Client.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITestAccountService, TestAccountService>();
        services.AddScoped<ITestAuthService, TestAuthService>();
        services.AddScoped<ITestSubscriptionService, TestSubscriptionService>();

        return services;
    }

    public static IServiceCollection AddTypedHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAccountHttpClient, AccountHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseAccountUrl);
        });

        services.AddHttpClient<IAuthHttpClient, AuthHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseAuthUrl);
        });

        services.AddHttpClient<ISubscriptionHttpClient, SubscriptionHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<ServiceUrlSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseSubscriptionUrl);
        });

        return services;
    }
}
