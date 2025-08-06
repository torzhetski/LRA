using LRA.Account.Presentation.Handlers;
using LRA.Account.Presentation.Interfaces;
using LRA.Account.Presentation.Services;
using LRA.Account.Presentation.Utils;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Infrastructure.HttpClients;
using LRA.Shared.Infrastructure.RabbitMq;
using LRA.Shared.Presentation.Handlers;

namespace LRA.Account.Presentation.Extensions;

public static class ServiceInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpClient<IKeycloakHttpClient, KeycloakHttpClient>();
        services.AddScoped<IKeycloakService, KeycloakService>();
        services.AddScoped<Seeder>();
        services.AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>();
        services.AddExceptionHandler<BaseExceptionHandler>();
        services.AddExceptionHandler<AccountExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
