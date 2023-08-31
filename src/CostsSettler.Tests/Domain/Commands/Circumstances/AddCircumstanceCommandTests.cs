using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Commands.Circumstances;

/// <summary>
/// Tests for AddCircumstanceCommand command.
/// </summary>
public class AddCircumstanceCommandTests
{
    private Mock<IChargeRepository> _chargeRepositoryMock { get; }
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    /// <summary>
    /// Creates new AddCircumstanceCommandTests.
    /// </summary>
    public AddCircumstanceCommandTests()
    {
        _chargeRepositoryMock = new Mock<IChargeRepository>();
        _circumstanceRepositoryMock = new Mock<ICircumstanceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _randomChargeFactory = new RandomChargeFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    /// <summary>
    /// Tests adding circumstance with valid data.
    /// </summary>
    [Fact]
    public void AddCircumstance_Success_Test()
    {
        var creditorId = Guid.NewGuid();
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        
        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var result = commandHandler.Handle(command, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Once);

        foreach (var userId in debtorsIds)
            _userRepositoryMock.Verify(repo =>
                repo.ExistsAsync(userId), Times.Once);

        _userRepositoryMock.Verify(repo =>
            repo.ExistsAsync(creditorId), Times.Once);

        Assert.True(result);
    }

    /// <summary>
    /// Tests adding circumstance with debtors list containing creditor.
    /// </summary>
    [Fact]
    public void AddCircumstance_DebtorsListContainsCreditor_Test()
    {
        var creditorId = Guid.NewGuid();
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), creditorId };

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Never);

        Assert.ThrowsAsync<ObjectReferenceException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests adding circumstance with debtor that does not exist.
    /// </summary>
    [Fact]
    public void AddCircumstance_DebtorDoesNotExist_Test()
    {
        var creditorId = Guid.NewGuid();
        var notExistingDebtorId = Guid.NewGuid();
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), notExistingDebtorId };

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(notExistingDebtorId))
            .ReturnsAsync(false);

        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Never);

        Assert.ThrowsAsync<ObjectNotFoundException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests adding circumstance with empty debtor id.
    /// </summary>
    [Fact]
    public void AddCircumstance_DebtorIdEmpty_Test()
    {
        var creditorId = Guid.NewGuid();
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.Empty };

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        
        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Never);

        Assert.ThrowsAsync<ObjectNotFoundException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests adding circumstance with creditor that does not exist.
    /// </summary>
    [Fact]
    public void AddCircumstance_CreditorDoesNotExist_Test()
    {
        var creditorId = Guid.NewGuid();
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(creditorId))
            .ReturnsAsync(false);

        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Never);

        Assert.ThrowsAsync<ObjectNotFoundException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests adding circumstance with empty creditor id.
    /// </summary>
    [Fact]
    public void AddCircumstance_CreditorIdEmpty_Test()
    {
        var creditorId = Guid.Empty;
        var debtorsIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(creditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var command = new AddCircumstanceCommand
        {
            Description = "testDesc",
            TotalAmount = (decimal)102.5,
            DebtorsIds = debtorsIds,
            Date = "2023-11-14",
            Time = "12:15",
            CreditorId = creditorId
        };

        var commandHandler = new AddCircumstanceCommand.AddCircumstanceCommandHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.AddAsync(It.IsAny<Circumstance>()),
            Times.Never);

        Assert.ThrowsAsync<ObjectNotFoundException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }
}
