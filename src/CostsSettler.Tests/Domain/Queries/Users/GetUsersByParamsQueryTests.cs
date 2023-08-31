using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CostsSettler.Domain.Dtos;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Profiles;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Queries.Users;

public class GetUsersByParamsQueryTests
{
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }
    private IMapper _mapper { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    public GetUsersByParamsQueryTests()
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

    [Fact]
    public void GetUsersByParams_Success_Test()
    {
        var users = _randomUserFactory.CreateCollection(5);

        _userRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        var query = new GetUsersByParamsQuery();

        var queryHandler = new GetUsersByParamsQuery.GetUsersByParamsQueryHandler(
            _userRepositoryMock.Object,
            _mapper
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _userRepositoryMock.Verify(repo =>
            repo.GetAllAsync(), Times.Once);

        var expected = _mapper.Map<ICollection<UserForListDto>>(users);

        Assert.Equal(expected.Count, result.Count);
        foreach (var item in expected)
            Assert.Contains(item, expected);
    }
}
