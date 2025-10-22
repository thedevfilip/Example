using Example.Domain.Entities;
using Example.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Example.Api.Features.Users.Registration;

internal sealed class RegisterUserHandler(UserManager<User> userManager)
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Result<RegisterUserResponse>> HandleAsync(RegisterUserRequest request)
    {
        User? existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return RegistrationErrors.EmailTaken;
        }

        var user = User.Create(request.Email, request.FirstName, request.LastName);

        // TODO: Print errors if creation fails
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // TODO: Log actual error
            return RegistrationErrors.InternalServerError;
        }

        return new RegisterUserResponse(user.Id, user.Email!);
    }
}
