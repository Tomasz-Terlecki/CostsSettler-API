using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;

/// <summary>
/// Gets charge by id.
/// </summary>
public class GetChargeByIdQuery : IRequest<Charge>
{
    /// <summary>
    /// Charge id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creates new GetChargeByIdQuery instance.
    /// </summary>
    /// <param name="id">Charge id.</param>
    public GetChargeByIdQuery(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// GetChargeByIdQuery handler.
    /// </summary>
    public class GetChargeByIdQueryHandler : IRequestHandler<GetChargeByIdQuery, Charge>
    {
        private readonly IChargeRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Creates new GetChargeByIdQueryHandler instance.
        /// </summary>
        /// <param name="repository">Repository that manages charges data.</param>
        /// <param name="userRepository">Repository that manages users data.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        public GetChargeByIdQueryHandler(IChargeRepository repository, IUserRepository userRepository,
            IIdentityService identityService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the GetChargeByIdQuery query.
        /// Checks if user is authorized, includes charge's creditor, debtor and circumstance.
        /// </summary>
        /// <param name="request">GetChargeByIdQuery to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Charges with given id.</returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        public async Task<Charge> Handle(GetChargeByIdQuery request, CancellationToken cancellationToken)
        {
            Charge charge = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Charge.Circumstance) })
                ?? throw new ObjectNotFoundException(typeof(Charge), request.Id);

            _identityService.CheckIfLoggedUserIsOneOf(new List<Guid> { charge.CreditorId, charge.DebtorId });

            charge.Creditor = await _userRepository.GetByIdAsync(charge.CreditorId)
                ?? throw new ObjectNotFoundException(typeof(User), charge.CreditorId);

            charge.Debtor = await _userRepository.GetByIdAsync(charge.DebtorId)
                ?? throw new ObjectNotFoundException(typeof(User), charge.DebtorId);
            
            return charge;
        }
    }
}
