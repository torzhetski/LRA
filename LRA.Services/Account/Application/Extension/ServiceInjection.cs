using LRA.Account.Application.Interfaces;
using LRA.Account.Application.Services;
using LRA.Account.Application.Services.Hosted;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Account.Application.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddHostedService<AccountRabbitMQConsumer>();

        return services;
    }
}
