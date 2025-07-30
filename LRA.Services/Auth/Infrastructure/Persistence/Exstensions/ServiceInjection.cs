using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrastructure.Persistence.Interfaces;
using LRA.Auth.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Auth.Infrastructure.Persistence.Extensions;

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
