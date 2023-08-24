using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostsSettler.Tests.Domain.Queries.Circumstances;
public class GetCircumstanceByIdQueryTests
{
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    public GetCircumstanceByIdQueryTests()
    {
        _circumstanceRepositoryMock = new Mock<ICircumstanceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _randomChargeFactory = new RandomChargeFactory();
        _randomCircumstanceFactory = new RandomCircumstanceFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    [Fact]
    public void GetChargeById_ChargeExists_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        var charge1 = new Charge
        {
            Id = Guid.NewGuid(),
            CircumstanceId = circumstanceId,
            CreditorId = creditorId1,
            DebtorId = debtorId1
        };
        var charge2 = new Charge
        {
            Id = Guid.NewGuid(),
            CircumstanceId = circumstanceId,
            CreditorId = creditorId2,
            DebtorId = debtorId2
        };

        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, charges: new List<Charge> { charge1, charge2 });

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetChargeByIdQuery(circumstanceId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);

        Assert.Equal(result, circumstance);
    }
}
