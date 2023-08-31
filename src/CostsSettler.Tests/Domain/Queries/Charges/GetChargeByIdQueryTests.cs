using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Queries.Charges;

/// <summary>
/// Tests of GetChargeByIdQuery query.
/// </summary>
public class GetChargeByIdQueryTests
{
    private Mock<IChargeRepository> _chargeRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    /// <summary>
    /// Creates new GetChargeByIdQueryTests instance.
    /// </summary>
    public GetChargeByIdQueryTests()
    {
        _chargeRepositoryMock = new Mock<IChargeRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _randomChargeFactory = new RandomChargeFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    /// <summary>
    /// Tests getting charge by id for exising charge.
    /// </summary>
    [Fact]
    public void GetChargeById_ChargeExists_Test()
    {
        var chargeId = Guid.NewGuid();
        var creditorId = Guid.NewGuid();
        var debtorId = Guid.NewGuid();

        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes 
        {
            CreditorId = creditorId, 
            DebtorId = debtorId
        });

        var creditor = _randomUserFactory.Create(creditorId);

        var debtor = _randomUserFactory.Create(debtorId);

        _chargeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(chargeId, It.IsAny<string[]>()))
            .ReturnsAsync(charge);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetChargeByIdQuery(chargeId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _chargeRepositoryMock.Object, 
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _chargeRepositoryMock.Verify(repo => 
            repo.GetByIdAsync(chargeId, It.IsAny<string[]>()), Times.Once);

        Assert.Equal(result, charge);
    }

    /// <summary>
    /// Tests getting charge by id for not exising charge.
    /// </summary>
    [Fact]
    public void GetChargeById_ChargeDoesNotExist_Test()
    {
        var chargeId = Guid.NewGuid();
        var creditorId = Guid.NewGuid();
        var debtorId = Guid.NewGuid();

        Charge? charge = null;

        var creditor = _randomUserFactory.Create(creditorId);

        var debtor = _randomUserFactory.Create(debtorId);

        _chargeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(chargeId, It.IsAny<string[]>()))
            .ReturnsAsync(charge);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetChargeByIdQuery(chargeId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _chargeRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;

        Assert.Contains(chargeId.ToString(), ex.Message);
    }

    /// <summary>
    /// Tests getting charge by id for unauthorized user.
    /// </summary>
    [Fact]
    public void GetChargeById_UserUnauthorized_Test()
    {
        var chargeId = Guid.NewGuid();
        var creditorId = Guid.NewGuid();
        var debtorId = Guid.NewGuid();

        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes 
        {
            CreditorId = creditorId, 
            DebtorId = debtorId
        });

        var creditor = _randomUserFactory.Create(creditorId);

        var debtor = _randomUserFactory.Create(debtorId);

        _chargeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(chargeId, It.IsAny<string[]>()))
            .ReturnsAsync(charge);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Throws<AuthorizationException>();

        var query = new GetChargeByIdQuery(chargeId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _chargeRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        Assert.ThrowsAsync<AuthorizationException>(
            () => queryHandler.Handle(query, CancellationToken.None));
    }

    /// <summary>
    /// Tests getting charge with creditor that is not found.
    /// </summary>
    [Fact]
    public void GetChargeById_CreditorNotFound_Test()
    {
        var chargeId = Guid.NewGuid();
        var creditorId = Guid.NewGuid();
        var debtorId = Guid.NewGuid();

        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes 
        {
            CreditorId = creditorId, 
            DebtorId = debtorId
        });

        User? creditor = null;

        var debtor = _randomUserFactory.Create(debtorId);

        _chargeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(chargeId, It.IsAny<string[]>()))
            .ReturnsAsync(charge);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetChargeByIdQuery(chargeId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _chargeRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;
        Assert.Contains(creditorId.ToString(), ex.Message);
    }

    /// <summary>
    /// Tests getting charge with debtor that is not found.
    /// </summary>
    [Fact]
    public void GetChargeById_DebtorNotFound_Test()
    {
        var chargeId = Guid.NewGuid();
        var creditorId = Guid.NewGuid();
        var debtorId = Guid.NewGuid();

        var charge = _randomChargeFactory.Create(chargeId, new ChargeAttributes 
        {
            CreditorId = creditorId, 
            DebtorId = debtorId
        });

        var creditor = _randomUserFactory.Create(creditorId);

        User? debtor = null;

        _chargeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(chargeId, It.IsAny<string[]>()))
            .ReturnsAsync(charge);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId))
            .ReturnsAsync(creditor);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId))
            .ReturnsAsync(debtor);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetChargeByIdQuery(chargeId);
        var queryHandler = new GetChargeByIdQuery.GetChargeByIdQueryHandler(
            _chargeRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;
        Assert.Contains(debtorId.ToString(), ex.Message);
    }
}
