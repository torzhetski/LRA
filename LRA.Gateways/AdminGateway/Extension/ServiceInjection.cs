using LRA.Shared.Presentation.Requirement;
using LRA.Shared.Utils.Constants;

namespace LRA.Gateways.AdminGateway.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        

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
