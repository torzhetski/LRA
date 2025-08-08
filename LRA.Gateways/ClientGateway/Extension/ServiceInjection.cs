using FluentValidation;
using LRA.Gateways.Client.Handlers;
using LRA.Gateways.Client.Interfaces;
using LRA.Gateways.Client.Services;
using LRA.Gateways.Client.Validators;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Requirement;
using LRA.Shared.Utils.Constants;
using Microsoft.Extensions.Options;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace LRA.Gateways.Client.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientAccountService, AccountService>();
        services.AddScoped<IClientAuthService, AuthService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IRoleProvider, AccountService>();
        services.AddExceptionHandler<ClientGatewayExceptionHandler>();
        services.AddProblemDetails();

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

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["JWTSettings:Authority"];
                    options.Audience = configuration["JWTSettings:Audience"];
                    options.RequireHttpsMetadata = configuration.GetValue<bool>("JWTSettings:RequireHttpsMetadata");
                });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ServerPolicy.RequireClient, policy =>
                policy.Requirements.Add(new SystemRoleRequirement(SystemRoles.Client)));
        });
        return services;
    }
}
