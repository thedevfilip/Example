using Example.Api;
using Example.Api.Features.Users.Info;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.Registration;
using Example.Infrastructure;
using Example.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddPresentation(configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();
app.MapLoginUser();
app.MapUserInfo();

app.Run();
