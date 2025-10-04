using Example.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Example.Api.Features.Users.Registration;

internal sealed class RegisterUserHandler(UserManager<User> userManager)
{
    private readonly UserManager<User> userManager = userManager;

    public async Task<RegisterUserResponse?> HandleAsync(RegisterUserRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        // TODO: Print errors if creation fails
        IdentityResult result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return null;
        }

        return new RegisterUserResponse(user.Id, user.Email!);
    }
}
