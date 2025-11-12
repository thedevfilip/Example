namespace Example.Api.Features.Users.Registration;

internal sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);
