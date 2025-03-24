using Microsoft.EntityFrameworkCore;
using Repository;

namespace WebApiExample.Extensions;

public static class AppExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        var migrations = db.Database.GetPendingMigrations();

        if (migrations.Any())
        {
            db.Database.Migrate();
        }
    }
}
