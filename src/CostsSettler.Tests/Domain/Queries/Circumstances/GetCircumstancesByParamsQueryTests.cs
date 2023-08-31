using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Profiles;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Queries.Circumstances;

/// <summary>
/// Tests of GetCircumstancesByParamsQuery query.
/// </summary>
public class GetCircumstancesByParamsQueryTests
{
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }
    private IMapper _mapper { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    /// <summary>
    /// Creates new GetCircumstancesByParamsQueryTests instance.
    /// </summary>
    public GetCircumstancesByParamsQueryTests()
    {
        _circumstanceRepositoryMock = new Mock<ICircumstanceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AutoMapperProfiles()))
            .CreateMapper();

        _randomChargeFactory = new RandomChargeFactory();
        _randomCircumstanceFactory = new RandomCircumstanceFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    /// <summary>
    /// Tests getting circumstances by params for success scenario.
    /// </summary>
    [Fact]
    public void GetCircumstancesByParams_Success_Test()
    {
        var userId = Guid.NewGuid();

        var circumstances = _randomCircumstanceFactory.CreateCollection(5);

        var query = new GetCircumstancesByParamsQuery {};

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByParamsAsync(query))
            .ReturnsAsync(circumstances);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(userId))
            .Verifiable();

        var queryHandler = new GetCircumstancesByParamsQuery.GetCircumstancesByParamsQueryHandler (
            _circumstanceRepositoryMock.Object,
            _mapper,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByParamsAsync(query), Times.Once);

        var expected = _mapper.Map<ICollection<CircumstanceForListDto>>(circumstances);

        Assert.Equal(expected.Count, result.Count);
        foreach (var item in expected)
            Assert.Contains(item, expected);
    }

    /// <summary>
    /// Tests getting circumstances by params for unauthorized user.
    /// </summary>
    [Fact]
    public void GetCircumstancesByParams_UserUnauthorized_Test()
    {
        var userId = Guid.NewGuid();

        var circumstances = _randomCircumstanceFactory.CreateCollection(5);

        var query = new GetCircumstancesByParamsQuery {};

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByParamsAsync(query))
            .ReturnsAsync(circumstances);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(userId))
            .Throws<AuthorizationException>();

        var queryHandler = new GetCircumstancesByParamsQuery.GetCircumstancesByParamsQueryHandler (
            _circumstanceRepositoryMock.Object,
            _mapper,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        Assert.ThrowsAsync<AuthorizationException>(
            () => queryHandler.Handle(query, CancellationToken.None));
    }
}
