namespace Example.Api.Features.Users.Registration;

public sealed record RegisterUserResponse(
    Guid UserId,
    string Email
);