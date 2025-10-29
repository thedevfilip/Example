using Example.Domain.Entities;
using Example.Domain.Interfaces;
using Example.Domain.Primitives;
using Example.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Result<LoginUserResponse>> HandleAsync(LoginUserRequest request)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return LoginUserErrors.InvalidCredentials;
        }

        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            return LoginUserErrors.InvalidCredentials;
        }

        UserOrganization? userOrganization = await context
            .Set<UserOrganization>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);

        string token = tokenProvider.Create(user, roles, userOrganization?.OrganizationId);

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
