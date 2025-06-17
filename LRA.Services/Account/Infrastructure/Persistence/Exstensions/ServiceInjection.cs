using LRA.Account.Application.Repositories;
using LRA.Account.Infrasturcture.Persistence.Interfaces;
using LRA.Account.Infrasturcture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Account.Infrasturcture.Persistence.Exstensions;

public static class ServiceInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IAccountDbContext, AccountDbContext>(options =>
            options.UseNpgsql(connString));

        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}
