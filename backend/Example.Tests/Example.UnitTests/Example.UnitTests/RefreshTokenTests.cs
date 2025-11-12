using Example.Domain.Entities;
using FluentAssertions;

namespace Example.UnitTests;

public class RefreshTokenTests
{
    private readonly User _user;
    private readonly RefreshToken _refreshToken;

    private const string ValidIpAddress = "127.0.0.1";
    private const string ValidUserAgent = "Mozilla/5.0";

    public RefreshTokenTests()
    {
        _user = new User();
        _refreshToken = RefreshToken.Create("token", _user, ValidIpAddress, ValidUserAgent);
    }

    [Fact]
    public void IsValidForClient_ShouldReturnTrue_WhenClientInfoMatchesAndTokenIsNotRevoked()
    {
        bool result = _refreshToken.IsValidForClient(ValidIpAddress, ValidUserAgent);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValidForClient_ShouldReturnFalse_WhenIpAddressDoesNotMatch()
    {
        string differentIpAddress = "192.168.1.1";

        bool result = _refreshToken.IsValidForClient(differentIpAddress, ValidUserAgent);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidForClient_ShouldReturnFalse_WhenUserAgentDoesNotMatch()
    {
        string differentUserAgent = "Chrome";

        bool result = _refreshToken.IsValidForClient(ValidIpAddress, differentUserAgent);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidForClient_ShouldReturnFalse_WhenTokenIsRevoked()
    {
        _refreshToken.Revoke();

        bool result = _refreshToken.IsValidForClient(ValidIpAddress, ValidUserAgent);

        result.Should().BeFalse();
    }

    [Fact]
    public void Revoke_ShouldSetIsRevokedToTrue()
    {
        _refreshToken.Revoke();

        _refreshToken.IsRevoked.Should().BeTrue();
    }
}
