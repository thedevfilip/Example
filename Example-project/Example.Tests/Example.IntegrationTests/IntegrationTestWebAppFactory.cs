using Example.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Example.IntegrationTests;

public sealed class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer sqlContainer = new MsSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder
        .ConfigureTestServices(services =>
        {
            ServiceDescriptor? descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlContainer.GetConnectionString()));
        });

    public async Task InitializeAsync()
    {
        await sqlContainer.StartAsync();

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(sqlContainer.GetConnectionString(),
                builder => builder.MigrationsAssembly("Example.Migrations"))
            .Options;

        await using var dbContext = new AppDbContext(options);
        await dbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await sqlContainer.DisposeAsync();
}
