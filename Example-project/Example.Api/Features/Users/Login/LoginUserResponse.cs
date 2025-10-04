namespace Example.Api.Features.Users.Login;

internal sealed record LoginUserResponse(
    string Email,
    string Token
);
