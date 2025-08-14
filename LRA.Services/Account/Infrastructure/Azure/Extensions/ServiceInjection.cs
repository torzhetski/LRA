using LRA.Account.Application.Interfaces;
using LRA.Account.Infrastructure.Azure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Account.Infrastructure.Azure.Extensions;

public static class ServiceInjection
{
    public static IServiceCollection AddAzureServices(this IServiceCollection services)
    {
        services.AddScoped<IAzureService, AzureService>();
        return services;
    }
}
