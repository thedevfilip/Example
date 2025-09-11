namespace Example.Api.Features.Users.Registration;

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);