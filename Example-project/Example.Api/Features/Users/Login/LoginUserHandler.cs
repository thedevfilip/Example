using Example.Domain.Entities;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Example.Api.Features.Users.Login;

internal sealed class LoginUserHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    TokenProvider tokenProvider,
    AppDbContext context)
{
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly UserManager<User> userManager = userManager;

    public async Task<LoginUserResponse?> HandleAsync(LoginUserRequest request)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return null;
        }

        IEnumerable<string> roles = await userManager.GetRolesAsync(user);

        string token = tokenProvider.Create(user, roles);

        var refreshToken = RefreshToken.Create(TokenProvider.CreateRefreshToken(), user);

        await context.Set<RefreshToken>()
            .AddAsync(refreshToken);

        await context.SaveChangesAsync();

        return new LoginUserResponse(token, refreshToken.Token);
    }
}
