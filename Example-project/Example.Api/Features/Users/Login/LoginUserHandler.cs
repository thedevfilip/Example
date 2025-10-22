using Example.Domain.Entities;
using Example.Domain.Interfaces;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Example.Api.Features.Users.Login;

internal sealed class LoginUserHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    TokenProvider tokenProvider,
    AppDbContext context,
    IClientInfoProvider clientInfoProvider,
    IHttpContextAccessor httpContextAccessor)
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<LoginUserResponse?> HandleAsync(LoginUserRequest request)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return null;
        }

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        string token = tokenProvider.Create(user, roles);

        HttpContext httpContext = httpContextAccessor.HttpContext!;

        var refreshToken = RefreshToken.Create(
            TokenProvider.CreateRefreshToken(),
            user,
            clientInfoProvider.GetClientIpAddress(httpContext),
            clientInfoProvider.GetUserAgent(httpContext));

        await context.AddAsync(refreshToken);

        await context.SaveChangesAsync();

        return new LoginUserResponse(token, refreshToken.Token);
    }
}
