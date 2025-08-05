using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Gateways.AdminGateway.Services;
using LRA.Shared.Abstractions.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Presentation.Requirement;
using LRA.Shared.Presentation.Services;
using LRA.Shared.Utils.Constants;
using Microsoft.Extensions.Options;

namespace LRA.Gateways.AdminGateway.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminAccountService, AccountService>();
        services.AddScoped<IAdminAuthService, AuthService>();
        services.AddScoped<IRoleProvider, AccountService>();

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

        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "http://keycloak:8080/realms/keycloak-auth";
            options.Audience = "account";
            options.RequireHttpsMetadata = false;
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(ServerPolicy.RequireSuperAdmin, policy =>
                policy.Requirements.Add(new SystemRoleRequirement(SystemRoles.SuperAdmin)));

            options.AddPolicy(ServerPolicy.RequireAdmin, policy =>
                policy.Requirements.Add(new SystemRoleRequirement(SystemRoles.Admin, SystemRoles.SuperAdmin)));
        });
        return services;
    }
}
