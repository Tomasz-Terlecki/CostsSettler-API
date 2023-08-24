using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Profiles;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Queries.Charges;
public class GetChargesByParamsQueryTests
{
    private Mock<IChargeRepository> _chargeRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }
    private IMapper _mapper { get; }

    private RandomChargeFactory _randomChargeFactory { get; }

    public GetChargesByParamsQueryTests()
    {
        _chargeRepositoryMock = new Mock<IChargeRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AutoMapperProfiles()))
            .CreateMapper();

        _randomChargeFactory = new RandomChargeFactory();
    }

    [Fact]
    public void GetChargesByParams_Success_Test()
    {
        var userId = Guid.NewGuid();

        var charges = _randomChargeFactory.CreateCollection(5);

        var query = new GetChargesByParamsQuery {};

        _chargeRepositoryMock
            .Setup(repo => repo.GetByParamsAsync(query))
            .ReturnsAsync(charges);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(userId))
            .Verifiable();

        var queryHandler = new GetChargesByParamsQuery.GetChargesByParamsQueryHandler(
            _chargeRepositoryMock.Object,
            _mapper,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _chargeRepositoryMock.Verify(repo =>
            repo.GetByParamsAsync(query), Times.Once);

        var expected = _mapper.Map<ICollection<ChargeForListDto>>(charges);

        foreach (var item in expected)
            Assert.Contains(item, expected);
    }

    [Fact]
    public void GetChargesByParams_UserUnauthorized_Test()
    {
        var userId = Guid.NewGuid();

        var charges = _randomChargeFactory.CreateCollection(5);

        var query = new GetChargesByParamsQuery { };

        _chargeRepositoryMock
            .Setup(repo => repo.GetByParamsAsync(query))
            .ReturnsAsync(charges);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(userId))
            .Throws<AuthorizationException>();

        var queryHandler = new GetChargesByParamsQuery.GetChargesByParamsQueryHandler(
            _chargeRepositoryMock.Object,
            _mapper,
            _identityServiceMock.Object
        );

        Assert.ThrowsAsync<AuthorizationException>(
            () => queryHandler.Handle(query, CancellationToken.None));
    }
}
