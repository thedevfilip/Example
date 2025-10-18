using System.Net;
using System.Net.Http.Json;
using Example.Api.Features.Users.Registration;
using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example.IntegrationTests;

public class RegisterTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Register_ShoulAdd_NewUserToDatabase()
    {
        RegisterUserRequest request = new(
            Email: $"asd@example.com",
            Password: "A@ssssss1!",
            FirstName: "test",
            LastName: "test");

        HttpResponseMessage response = await Client.PostAsJsonAsync("/api/users/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        User? created = await ExecuteScopedAsync(async sp =>
        {
            AppDbContext db = sp.GetRequiredService<AppDbContext>();
            return await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == request.Email);
        });

        Assert.NotNull(created);
    }
}
