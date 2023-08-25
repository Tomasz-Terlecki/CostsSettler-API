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
public class VoteForChargeCommandTests
{
    private Mock<IChargeRepository> _chargeRepositoryMock { get; }
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }
    private Mock<IMediator> _mediatorMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    public VoteForChargeCommandTests()
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

    [Theory]
    [InlineData(ChargeStatus.None, CircumstanceStatus.None, ChargeVote.Accept)]
    [InlineData(ChargeStatus.None, CircumstanceStatus.None, ChargeVote.Reject)]
    [InlineData(ChargeStatus.New, CircumstanceStatus.New, ChargeVote.Accept)]
    [InlineData(ChargeStatus.New, CircumstanceStatus.New, ChargeVote.Reject)]
    [InlineData(ChargeStatus.Rejected, CircumstanceStatus.Rejected, ChargeVote.Accept)]
    [InlineData(ChargeStatus.Rejected, CircumstanceStatus.Rejected, ChargeVote.Reject)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.Accepted, ChargeVote.Accept)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.Accepted, ChargeVote.Reject)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.PartiallyAccepted, ChargeVote.Accept)]
    [InlineData(ChargeStatus.Accepted, CircumstanceStatus.PartiallyAccepted, ChargeVote.Reject)]
    public void VoteForCharge_ValidCircumstanceStatusAndChargeStatus_Test(
        ChargeStatus chargeStatus, CircumstanceStatus circumstanceStatus, ChargeVote chargeVote)
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = circumstanceStatus });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Debtor = _randomUserFactory.Create(),
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
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.DebtorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new VoteForChargeCommand { ChargeId = chargeId, ChargeVote = chargeVote };

        var commandHandler = new VoteForChargeCommand.VoteForChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        var result = commandHandler.Handle(command, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Circumstance>(circumstance =>
                chargeVote == ChargeVote.Accept
                    ? circumstance.CircumstanceStatus == CircumstanceStatus.Accepted ||
                      circumstance.CircumstanceStatus == CircumstanceStatus.PartiallyAccepted
                    : circumstance.CircumstanceStatus == CircumstanceStatus.Rejected)),
             Times.Once);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.Is<Charge>(charge =>
                charge.ChargeStatus == (chargeVote == ChargeVote.Accept
                    ? ChargeStatus.Accepted
                    : ChargeStatus.Rejected))),
            Times.Once);

        _identityServiceMock.Verify(repo =>
            repo.CheckEqualityWithLoggedUserId(It.IsAny<Guid>()),
            Times.Once);

        Assert.True(result);
    }

    [Theory]
    [InlineData(ChargeStatus.Settled, CircumstanceStatus.PartiallyAccepted, ChargeVote.Reject)]
    [InlineData(ChargeStatus.New, CircumstanceStatus.Settled, ChargeVote.Reject)]
    [InlineData(ChargeStatus.New, CircumstanceStatus.PartiallySettled, ChargeVote.Reject)]
    public void VoteForCharge_InValidCircumstanceStatusOrChargeStatus_Test(
        ChargeStatus chargeStatus, CircumstanceStatus circumstanceStatus, ChargeVote chargeVote)
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = circumstanceStatus });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Debtor = _randomUserFactory.Create(),
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
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.DebtorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new VoteForChargeCommand { ChargeId = chargeId, ChargeVote = chargeVote };

        var commandHandler = new VoteForChargeCommand.VoteForChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        Assert.ThrowsAsync<DomainLogicException>(
            () => commandHandler.Handle(command, CancellationToken.None));

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.IsAny<Circumstance>()), Times.Never);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.IsAny<Charge>()), Times.Never);
    }

    [Fact]
    public void VoteForCharge_InValidChargeVote_Test()
    {
        var chargeId = Guid.NewGuid();
        var circumstanceId = Guid.NewGuid();
        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, new CircumstanceAttributes { CircumstanceStatus = CircumstanceStatus.New });
        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes
        {
            CircumstanceId = circumstanceId,
            Circumstance = circumstance,
            Debtor = _randomUserFactory.Create(),
            ChargeStatus = ChargeStatus.New
        });
        circumstance.Charges = new List<Charge> { charge };

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetChargeByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(charge);

        _mediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<GetCircumstanceByIdQuery>(), CancellationToken.None))
            .ReturnsAsync(circumstance);

        _identityServiceMock
            .Setup(service => service.CheckEqualityWithLoggedUserId(charge.DebtorId))
            .Verifiable();

        _circumstanceRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Circumstance>()))
            .ReturnsAsync(true);

        _chargeRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<Charge>()))
            .ReturnsAsync(true);

        var command = new VoteForChargeCommand { ChargeId = chargeId, ChargeVote = ChargeVote.None };

        var commandHandler = new VoteForChargeCommand.VoteForChargeCommandHandler(
            _mediatorMock.Object,
            _chargeRepositoryMock.Object,
            _identityServiceMock.Object,
            _circumstanceRepositoryMock.Object
        );

        Assert.ThrowsAsync<ObjectReferenceException>(
            () => commandHandler.Handle(command, CancellationToken.None));

        _circumstanceRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.IsAny<Circumstance>()), Times.Never);

        _chargeRepositoryMock.Verify(repo =>
            repo.UpdateAsync(It.IsAny<Charge>()), Times.Never);
    }
}
