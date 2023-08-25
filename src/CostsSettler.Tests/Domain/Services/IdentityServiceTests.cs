using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace CostsSettler.Tests.Domain.Services;
public class IdentityServiceTests
{
    private Mock<IHttpContextAccessor> _httpContextAccessorMock { get; }
    private Mock<HttpContext> _httpContextMock { get; }

    public IdentityServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextMock = new Mock<HttpContext>();
    }

    [Fact]
    public void CheckEqualityWithLoggedUserId_AuthorizedUser_Test()
    {
        var userId = Guid.NewGuid();

        _httpContextMock
            .Setup(context => context.User)
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("appId", userId.ToString()) }, "Bearer")));

        _httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(_httpContextMock.Object);

        var identityService = new IdentityService(_httpContextAccessorMock.Object);

        identityService.CheckEqualityWithLoggedUserId(userId);
    }

    [Fact]
    public void CheckEqualityWithLoggedUserId_UnauthorizedUser_Test()
    {
        var userId = Guid.NewGuid();

        _httpContextMock
            .Setup(context => context.User)
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("appId", userId.ToString()) }, "Bearer")));

        _httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(_httpContextMock.Object);

        var identityService = new IdentityService(_httpContextAccessorMock.Object);

        Assert.Throws<AuthorizationException>(
            () => identityService.CheckEqualityWithLoggedUserId(Guid.NewGuid()));
    }

    [Fact]
    public void CheckIfLoggedUserIsOneOf_AuthorizedUser_Test()
    {
        var userId = Guid.NewGuid();
        var userIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), userId };

        _httpContextMock
            .Setup(context => context.User)
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("appId", userId.ToString()) }, "Bearer")));

        _httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(_httpContextMock.Object);

        var identityService = new IdentityService(_httpContextAccessorMock.Object);

        identityService.CheckIfLoggedUserIsOneOf(userIds);
    }

    [Fact]
    public void CheckIfLoggedUserIsOneOf_UnauthorizedUser_Test()
    {
        var userId = Guid.NewGuid();
        var userIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        _httpContextMock
            .Setup(context => context.User)
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("appId", userId.ToString()) }, "Bearer")));

        _httpContextAccessorMock
            .Setup(accessor => accessor.HttpContext)
            .Returns(_httpContextMock.Object);

        var identityService = new IdentityService(_httpContextAccessorMock.Object);

        Assert.Throws<AuthorizationException>(
            () => identityService.CheckIfLoggedUserIsOneOf(userIds));
    }
}
