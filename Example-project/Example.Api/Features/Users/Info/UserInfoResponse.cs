namespace Example.Api.Features.Users.Info;

public sealed record UserInfoResponse(
    Guid UserId,
    string Email,
    string UserName,
    string? FirstName,
    string? LastName
);