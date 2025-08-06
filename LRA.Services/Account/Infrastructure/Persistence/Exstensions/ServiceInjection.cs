using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Infrastructure.Persistence.Interfaces;
using LRA.Account.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Account.Infrastructure.Persistence.Extensions;

public static class ServiceInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IAccountDbContext, AccountDbContext>(options =>
            options.UseNpgsql(connString));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IVerificationRepository, VerificationRepository>();

        return services;
    }
}
