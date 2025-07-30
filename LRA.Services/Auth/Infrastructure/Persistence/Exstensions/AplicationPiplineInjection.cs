using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LRA.Auth.Infrastructure.Persistence.Extensions;

public static class ApplicationPipelineInjection
{
    public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app)
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
