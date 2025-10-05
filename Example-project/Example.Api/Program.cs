using Example.Api;
using Example.Api.Features.Users.Info;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.Registration;
using Example.Infrastructure;
using Example.Infrastructure.Seeders;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

services.AddPresentation(configuration);
services.AddInfrastructure(configuration);

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    DatabaseSeeder seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();
app.MapLoginUser();
app.MapUserInfo();

await app.RunAsync();
