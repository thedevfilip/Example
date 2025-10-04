namespace Example.Api.Features.Users.Info;

internal sealed record UserInfoResponse(
    Guid UserId,
    string Email,
    string UserName,
    string? FirstName,
    string? LastName
);
