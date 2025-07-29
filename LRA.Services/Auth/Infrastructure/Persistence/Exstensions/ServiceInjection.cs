using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using LRA.Auth.Infrasturcture.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Auth.Infrasturcture.Persistence.Exstensions;

public static class ServiceInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IAuthDbContext, AuthDbContext>(options =>
            options.UseNpgsql(connString));

        services.AddScoped<IVerificationRepository, VerificationRepository>();

        return services;
    }
}
