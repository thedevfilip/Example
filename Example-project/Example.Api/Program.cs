using Example.Api;
using Example.Api.Features.Organizations.Registration;
using Example.Api.Features.Users.Info;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.RefreshTokenLogin;
using Example.Api.Features.Users.Registration;
using Example.Infrastructure;
using Example.Infrastructure.Seeders;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
ConfigureWebHostBuilder webHost = builder.WebHost;

webHost.ConfigureKestrel(options => options.AddServerHeader = false);

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

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();
app.MapLoginUser();
app.MapRefreshToken();
app.MapUserInfo();
app.MapRegisterOrganization();

await app.RunAsync();

public partial class Program { }
