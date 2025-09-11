using Example.Api;
using Example.Api.Features.Users.Registration;
using Example.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddInfrastructure(configuration);
services.AddPresentation();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapRegisterUser();

app.Run();
