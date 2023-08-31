using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace CostsSettler.Tests.Domain.Services;

/// <summary>
/// Tests of IdentityService service.
/// </summary>
public class IdentityServiceTests
{
    private Mock<IHttpContextAccessor> _httpContextAccessorMock { get; }
    private Mock<HttpContext> _httpContextMock { get; }

    /// <summary>
    /// Creates new IdentityServiceTests instance.
    /// </summary>
    public IdentityServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextMock = new Mock<HttpContext>();
    }

    /// <summary>
    /// Checks equality with logged user id for authorized user.
    /// </summary>
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

    /// <summary>
    /// Checks equality with logged user id for unauthorized user.
    /// </summary>
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

    /// <summary>
    /// Checks given user id is one of a list of user ids for authorized user.
    /// </summary>
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

    /// <summary>
    /// Checks given user id is one of a list of user ids for unauthorized user.
    /// </summary>
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
