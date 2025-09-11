namespace Example.Api.Features.Users.Login;

public sealed record LoginUserResponse(
    string Email,
    string Token
);