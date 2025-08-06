using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using LRA.Subscription.Infrasturcture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Subscription.Infrasturcture.Persistence.Extension;

public static class ServiceInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ISubscriptionDbContext, SubscriptionDbContext>(options =>
            options.UseNpgsql(connString));

        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
