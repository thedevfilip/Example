using Example.Api;
using Example.Api.Features.Users.Login;
using Example.Api.Features.Users.Registration;
using Example.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddPresentation(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();
app.MapLoginUser();

app.Run();
