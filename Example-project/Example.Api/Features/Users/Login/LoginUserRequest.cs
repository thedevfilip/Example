namespace Example.Api.Features.Users.Login;

internal sealed record LoginUserRequest(
    string Email,
    string Password
);
