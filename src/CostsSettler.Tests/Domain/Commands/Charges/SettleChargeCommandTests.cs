using CostsSettler.Domain.Commands;
using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using MediatR;
using Moq;

namespace CostsSettler.Tests.Domain.Commands.Charges;

/// <summary>
/// Tests for SettlerChargeCommand command.
/// </summary>
public class SettleChargeCommandTests
{
    private Mock<IChargeRepository> _chargeRepositoryMock { get; }
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }
    private Mock<IMediator> _mediatorMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    /// <summary>
    /// Creates new SettleChargeCommandTests.
    /// </summary>
    public SettleChargeCommandTests()
    {
        _chargeRepositoryMock = new Mock<IChargeRepository>();
        _circumstanceRepositoryMock = new Mock<ICircumstanceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();
        _mediatorMock = new Mock<IMediator>();

        _randomChargeFactory = new RandomChargeFactory();
        _randomCircumstanceFactory = new RandomCircumstanceFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    /// <summary>
    /// Tests settle charge functionality for valid charge status and circumstance status.
    /// </summary>
    /// <param name="chargeStatus">Status of charge to be settled.</param>
    /// <param name="circumstanceStatus">Status of circumstance to be settled.</param>
    [Theory]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.Accepted)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.PartiallySettled)]
    public void SettleCharge_SettleValidChargeStatusAndCircumstanceStatus_Test(ChargeStatus chargeStatus, CircumstanceStatus circumstanceStatus)
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = circumstanceStatus });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Creditor = _randomUserFactory.Create(),
            ChargeStatus = chargeStatus
        });
        circumstance.Charges = new List<Charge> { charge };
        
        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetChargeByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(charge);

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetCircumstanceByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(circumstance);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.CreditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new SettleChargeCommand { ChargeId = chargeId };

        var commandHandler = new SettleChargeCommand.SettleChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        var result = commandHandler.Handle(command, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Circumstance>(circumstance => 
                circumstance.CircumstanceStatus == CircumstanceStatus.Settled ||
                circumstance.CircumstanceStatus == CircumstanceStatus.PartiallySettled)),
            Times.Once);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Charge>(charge =>
                charge.ChargeStatus == ChargeStatus.Settled)),
            Times.Once);

        _identityServiceMock.Verify(service =>
            service.CheckEqualityWithLoggedUserId(It.IsAny<Guid>()),
            Times.Once);

        Assert.True(result);
    }

    /// <summary>
    /// Tests settle charge functionality for invalid charge status or circumstance status.
    /// </summary>
    /// <param name="chargeStatus">Status of charge to be settled.</param>
    /// <param name="circumstanceStatus">Status of circumstance to be settled.</param>
    [Theory]
    [InlineData(ChargeStatus.None, CircumstanceStatus.Accepted)]
    [InlineData(ChargeStatus.New, CircumstanceStatus.Accepted)]
    [InlineData(ChargeStatus.Rejected, CircumstanceStatus.Accepted)]
    [InlineData(ChargeStatus.Settled, CircumstanceStatus.Accepted)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.None)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.New)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.PartiallyAccepted)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.Settled)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.Rejected)]
    public void SettleCharge_SettleInvalidChargeOrCircumstanceStatus_Test(ChargeStatus chargeStatus, CircumstanceStatus circumstanceStatus)
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = circumstanceStatus });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Creditor = _randomUserFactory.Create(),
            ChargeStatus = chargeStatus
        });
        circumstance.Charges = new List<Charge> { charge };

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetChargeByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(charge);

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetCircumstanceByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(circumstance);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.CreditorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new SettleChargeCommand { ChargeId = chargeId };

        var commandHandler = new SettleChargeCommand.SettleChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Circumstance>(circumstance =>
                circumstance.CircumstanceStatus == CircumstanceStatus.Settled ||
                circumstance.CircumstanceStatus == CircumstanceStatus.PartiallySettled)),
            Times.Never);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Charge>(charge =>
                charge.ChargeStatus == ChargeStatus.Settled)),
            Times.Never);

        Assert.ThrowsAsync<DomainLogicException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }

    /// <summary>
    /// Tests settle charge functionality for unauthorized user.
    /// </summary>
    [Fact]
    public void SettleCharge_UnauthorizedUser_Test()
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = CircumstanceStatus.Accepted });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Creditor = _randomUserFactory.Create(),
            ChargeStatus = ChargeStatus.Accepted
        });
        circumstance.Charges = new List<Charge> { charge };

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetChargeByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(charge);

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetCircumstanceByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(circumstance);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.CreditorId))
            .Throws<AuthorizationException>();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new SettleChargeCommand { ChargeId = chargeId };

        var commandHandler = new SettleChargeCommand.SettleChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Circumstance>(circumstance =>
                circumstance.CircumstanceStatus == CircumstanceStatus.Settled ||
                circumstance.CircumstanceStatus == CircumstanceStatus.PartiallySettled)),
            Times.Never);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Charge>(charge =>
                charge.ChargeStatus == ChargeStatus.Settled)),
            Times.Never);

        Assert.ThrowsAsync<AuthorizationException>(
            () => commandHandler.Handle(command, CancellationToken.None));
    }
}
