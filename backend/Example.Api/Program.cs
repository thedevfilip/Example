using Example.Api;
using Example.Api.Contexts;
using Example.Domain.Contexts;
using Example.Infrastructure;
using Example.Infrastructure.Seeders;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
ConfigureWebHostBuilder webHost = builder.WebHost;

webHost.ConfigureKestrel(options => options.AddServerHeader = false);

services.AddScoped<IOrganizationContext, OrganizationContext>();

services.AddPresentation(configuration);
services.AddInfrastructure(configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

using (IServiceScope scope = app.Services.CreateScope())
{
    DatabaseSeeder seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.ConfigurePresentation();

await app.RunAsync();

public partial class Program { }
