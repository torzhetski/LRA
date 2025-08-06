using FluentValidation;
using LRA.Gateways.Client.Handlers;
using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Services;
using LRA.Gateways.Client.Validators;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace LRA.Gateways.Client.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITestSubscriptionService, TestSubscriptionService>();
        services.AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddExceptionHandler<ClientGatewayExceptionHandler>();
        services.AddProblemDetails();
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://keycloak:8080/realms/keycloak-auth";
                options.Audience = "account";
                options.RequireHttpsMetadata = false;
            });
        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddTypedHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IAccountHttpClient, AccountHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<AccountingEndpointsSettings>>().Value;
            client.BaseAddress = new Uri(options.Url);
        });

        services.AddHttpClient<IAuthHttpClient, AuthHttpClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<AuthEndpointsSettings>>().Value;
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
