namespace Example.Api.Features.Users.Login;

public sealed record LoginUserRequest(
    string Email,
    string Password
);