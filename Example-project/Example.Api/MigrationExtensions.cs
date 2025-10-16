using Example.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Example.Api;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}
