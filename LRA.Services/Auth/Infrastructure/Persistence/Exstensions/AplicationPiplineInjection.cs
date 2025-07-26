using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace LRA.Auth.Infrasturcture.Persistence.Exstensions;

public static class AplicationPiplineInjection
{
    public static async Task<IApplicationBuilder> ApplyMigrations(this  IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            if ((await db.Database.GetPendingMigrationsAsync()).Any())
            {
                db.Database.Migrate();
            }

            return app;
        }
    }
}
