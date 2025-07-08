using LRA.Auth.Application.Interfaces;
using LRA.Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Auth.Application.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITestService, TestService>();

        return services;
    }
}
