using FluentValidation;
using LRA.Gateways.Client.Handlers;
using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Services;
using LRA.Gateways.Client.Validators;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Handlers;
using Microsoft.Extensions.Options;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace LRA.Gateways.Client.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITestAuthService, TestAuthService>();
        services.AddScoped<ITestSubscriptionService, TestSubscriptionService>();
        services.AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddExceptionHandler<ClientGatewayExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddTypedHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAccountHttpClient, AccountHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<AccountServiceSettings>>().Value;
            client.BaseAddress = new Uri(options.Url);
        });

        services.AddHttpClient<IAuthHttpClient, AuthHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<AuthServiceSettings>>().Value;
            client.BaseAddress = new Uri(options.Url);
        });

        services.AddHttpClient<ISubscriptionHttpClient, SubscriptionHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<SubscriptionServiceSettings>>().Value;
            client.BaseAddress = new Uri(options.Url);
        });

        return services;
    }
}
