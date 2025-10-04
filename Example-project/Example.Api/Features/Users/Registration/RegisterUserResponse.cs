namespace Example.Api.Features.Users.Registration;

internal sealed record RegisterUserResponse(
    Guid UserId,
    string Email
);
