using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using CostsSettler.Tests.Helpers;
using Moq;

namespace CostsSettler.Tests.Domain.Queries.Circumstances;

/// <summary>
/// Tests of GetCircumstanceByIdQuery query.
/// </summary>
public class GetCircumstanceByIdQueryTests
{
    private Mock<ICircumstanceRepository> _circumstanceRepositoryMock { get; }
    private Mock<IUserRepository> _userRepositoryMock { get; }
    private Mock<IIdentityService> _identityServiceMock { get; }

    private RandomChargeFactory _randomChargeFactory { get; }
    private RandomCircumstanceFactory _randomCircumstanceFactory { get; }
    private RandomUserFactory _randomUserFactory { get; }

    /// <summary>
    /// Creates new GetCircumstanceByIdQueryTests instance.
    /// </summary>
    public GetCircumstanceByIdQueryTests()
    {
        _circumstanceRepositoryMock = new Mock<ICircumstanceRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _identityServiceMock = new Mock<IIdentityService>();

        _randomChargeFactory = new RandomChargeFactory();
        _randomCircumstanceFactory = new RandomCircumstanceFactory();
        _randomUserFactory = new RandomUserFactory();
    }

    /// <summary>
    /// Tests getting circumstance by id for existing circumstance.
    /// </summary>
    [Fact]
    public void GetCircumstanceById_CircumstanceExists_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        var creditor1 = _randomUserFactory.Create(creditorId1);
        var creditor2 = _randomUserFactory.Create(creditorId2);
        var debtor1 = _randomUserFactory.Create(debtorId1);
        var debtor2 = _randomUserFactory.Create(debtorId2);

        var charge1 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor1,
            Debtor = debtor1
        });
        var charge2 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor2,
            Debtor = debtor2
        });

        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, 
            new CircumstanceAttributes 
            { 
                Charges = new List<Charge> { charge1, charge2 } 
            });

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId2))
            .ReturnsAsync(creditor2);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId1))
            .ReturnsAsync(debtor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId2))
            .ReturnsAsync(debtor2);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetCircumstanceByIdQuery(circumstanceId);
        var queryHandler = new GetCircumstanceByIdQuery.GetCircumstanceByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var result = queryHandler.Handle(query, CancellationToken.None).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);

        Assert.Equal(result, circumstance);
    }

    /// <summary>
    /// Tests getting circumstance by id for circumstance that does not exist.
    /// </summary>
    [Fact]
    public void GetCircumstanceById_CircumstanceDoesNotExist_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        var creditor1 = _randomUserFactory.Create(creditorId1);
        var creditor2 = _randomUserFactory.Create(creditorId2);
        var debtor1 = _randomUserFactory.Create(debtorId1);
        var debtor2 = _randomUserFactory.Create(debtorId2);

        var charge1 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor1,
            Debtor = debtor1
        });
        var charge2 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor2,
            Debtor = debtor2
        });

        Circumstance? circumstance = null;

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId2))
            .ReturnsAsync(creditor2);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId1))
            .ReturnsAsync(debtor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId2))
            .ReturnsAsync(debtor2);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Verifiable();

        var query = new GetCircumstanceByIdQuery(circumstanceId);
        var queryHandler = new GetCircumstanceByIdQuery.GetCircumstanceByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);

        Assert.Contains(circumstanceId.ToString(), ex.Message);
    }

    /// <summary>
    /// Tests getting circumstance by id for unauthorized user.
    /// </summary>
    [Fact]
    public void GetCircumstanceById_UserUnauthorized_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        var creditor1 = _randomUserFactory.Create(creditorId1);
        var creditor2 = _randomUserFactory.Create(creditorId2);
        var debtor1 = _randomUserFactory.Create(debtorId1);
        var debtor2 = _randomUserFactory.Create(debtorId2);

        var charge1 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor1,
            Debtor = debtor1
        });
        var charge2 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor2,
            Debtor = debtor2
        });

        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, 
            new CircumstanceAttributes 
            { 
                Charges = new List<Charge> { charge1, charge2 } 
            });

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId2))
            .ReturnsAsync(creditor2);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId1))
            .ReturnsAsync(debtor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId2))
            .ReturnsAsync(debtor2);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Throws<AuthorizationException>();

        var query = new GetCircumstanceByIdQuery(circumstanceId);
        var queryHandler = new GetCircumstanceByIdQuery.GetCircumstanceByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        Assert.ThrowsAsync<AuthorizationException>(
            () => queryHandler.Handle(query, CancellationToken.None));

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);
    }

    /// <summary>
    /// Tests getting circumstance by id for circumstance's creditor not found.
    /// </summary>
    [Fact]
    public void GetCircumstanceById_CreditorNotFound_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        User? creditor1 = null;
        var creditor2 = _randomUserFactory.Create(creditorId2);
        var debtor1 = _randomUserFactory.Create(debtorId1);
        var debtor2 = _randomUserFactory.Create(debtorId2);

        var charge1 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            CreditorId = creditorId1,
            Debtor = debtor1
        });
        var charge2 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor2,
            Debtor = debtor2
        });

        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, 
            new CircumstanceAttributes 
            { 
                Charges = new List<Charge> { charge1, charge2 } 
            });

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId2))
            .ReturnsAsync(creditor2);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId1))
            .ReturnsAsync(debtor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId2))
            .ReturnsAsync(debtor2);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Throws<AuthorizationException>();

        var query = new GetCircumstanceByIdQuery(circumstanceId);
        var queryHandler = new GetCircumstanceByIdQuery.GetCircumstanceByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);

        Assert.Contains(creditorId1.ToString(), ex.Message);
    }

    /// <summary>
    /// Tests getting circumstance by id for circumstance's debtor not found.
    /// </summary>
    [Fact]
    public void GetCircumstanceById_DebtorNotFound_Test()
    {
        var circumstanceId = Guid.NewGuid();
        var creditorId1 = Guid.NewGuid();
        var creditorId2 = Guid.NewGuid();
        var debtorId1 = Guid.NewGuid();
        var debtorId2 = Guid.NewGuid();

        var creditor1 = _randomUserFactory.Create(creditorId1);
        var creditor2 = _randomUserFactory.Create(creditorId2);
        User? debtor1 = null;
        var debtor2 = _randomUserFactory.Create(debtorId2);

        var charge1 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor1,
            DebtorId = debtorId1
        });
        var charge2 = _randomChargeFactory.Create(Guid.NewGuid(), new ChargeAttributes 
        {
            CircumstanceId = circumstanceId,
            Creditor = creditor2,
            Debtor = debtor2
        });

        var circumstance = _randomCircumstanceFactory.Create(circumstanceId, 
            new CircumstanceAttributes 
            { 
                Charges = new List<Charge> { charge1, charge2 } 
            });

        _circumstanceRepositoryMock
            .Setup(repo => repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()))
            .ReturnsAsync(circumstance);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId1))
            .ReturnsAsync(creditor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(creditorId2))
            .ReturnsAsync(creditor2);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId1))
            .ReturnsAsync(debtor1);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(debtorId2))
            .ReturnsAsync(debtor2);

        _identityServiceMock
            .Setup(service => service.CheckIfLoggedUserIsOneOf(It.IsAny<Guid[]>()))
            .Throws<AuthorizationException>();

        var query = new GetCircumstanceByIdQuery(circumstanceId);
        var queryHandler = new GetCircumstanceByIdQuery.GetCircumstanceByIdQueryHandler(
            _circumstanceRepositoryMock.Object,
            _userRepositoryMock.Object,
            _identityServiceMock.Object
        );

        var ex = Assert.ThrowsAsync<ObjectNotFoundException>(
            () => queryHandler.Handle(query, CancellationToken.None)).Result;

        _circumstanceRepositoryMock.Verify(repo =>
            repo.GetByIdAsync(circumstanceId, It.IsAny<string[]>()), Times.Once);

        Assert.Contains(debtorId1.ToString(), ex.Message);
    }
}
