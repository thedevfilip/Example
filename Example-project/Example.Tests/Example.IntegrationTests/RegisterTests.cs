using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Example.Api.Features.Users.Registration;
using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Example.IntegrationTests;

public class RegisterTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Register_ShoulAdd_NewUserToDatabase()
    {
        RegisterUserRequest request = new(
            Email: $"a@example.com",
            Password: "A@ssssss1!",
            FirstName: "test",
            LastName: "test");

        HttpResponseMessage response = await Client.PostAsJsonAsync("/api/users/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        RegisterUserResponse? payload = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();

        payload.Should().NotBeNull();
        payload!.Email.Should().Be(request.Email);

        User? created = await ExecuteScopedAsync(async sp =>
        {
            AppDbContext db = sp.GetRequiredService<AppDbContext>();
            return await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == request.Email);
        });

        created.Should().NotBeNull();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturn_BadRequest()
    {
        string email = $"b@example.com";

        // Seed existing user directly to avoid consuming rate limit
        await ExecuteScopedAsync(async sp =>
        {
            UserManager<User> userManager = sp.GetRequiredService<UserManager<User>>();
            var user = new User { UserName = email, Email = email, FirstName = "test", LastName = "test" };
            IdentityResult result = await userManager.CreateAsync(user, "A@ssssss1!");
            result.Succeeded.Should().BeTrue("Seeding user should succeed");
        });

        RegisterUserRequest duplicate = new(
            Email: email,
            Password: "A@ssssss1!",
            FirstName: "test",
            LastName: "test");

        HttpResponseMessage response = await Client.PostAsJsonAsync("/api/users/register", duplicate);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithInvalidPassword_ShouldReturn_BadRequest()
    {
        RegisterUserRequest request = new(
            Email: $"c@example.com",
            Password: "weak",
            FirstName: "test",
            LastName: "test");

        HttpResponseMessage response = await Client.PostAsJsonAsync("/api/users/register", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
