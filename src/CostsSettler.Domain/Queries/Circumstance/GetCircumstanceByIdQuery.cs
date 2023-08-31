using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Queries;

/// <summary>
/// Gets circumstance by id.
/// </summary>
public class GetCircumstanceByIdQuery : IRequest<Circumstance>
{
    /// <summary>
    /// Circumstance id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creates new GetCircumstanceByIdQuery instance.
    /// </summary>
    /// <param name="id">Circumstance id.</param>
    public GetCircumstanceByIdQuery(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// GetCircumstanceByIdQuery handler.
    /// </summary>
    public class GetCircumstanceByIdQueryHandler : IRequestHandler<GetCircumstanceByIdQuery, Circumstance>
    {
        private readonly ICircumstanceRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Creates new GetCircumstanceByIdQueryHandler instance.
        /// </summary>
        /// <param name="repository">Repository that manages circumstances data.</param>
        /// <param name="userRepository">Repository that manages users data.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        public GetCircumstanceByIdQueryHandler(ICircumstanceRepository repository, 
            IUserRepository userRepository, IIdentityService identityService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the GetCircumstanceByIdQuery query.
        /// Checks if user is authorized, includes charges, debtors and creditors.
        /// </summary>
        /// <param name="request">GetCircumstanceByIdQuery to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Circumstance with given id.</returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        public async Task<Circumstance> Handle(GetCircumstanceByIdQuery request, CancellationToken cancellationToken)
        {
            var circumstance = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Circumstance.Charges) });

            if (circumstance == null)
                throw new ObjectNotFoundException(typeof(Circumstance), request.Id);

            foreach (var charge in circumstance.Charges ?? new List<Charge>())
            {
                charge.Creditor = await _userRepository.GetByIdAsync(charge.CreditorId) 
                    ?? throw new ObjectNotFoundException(typeof(User), charge.CreditorId);
                
                charge.Debtor = await _userRepository.GetByIdAsync(charge.DebtorId)
                    ?? throw new ObjectNotFoundException(typeof(User), charge.DebtorId);
            }

            _identityService.CheckIfLoggedUserIsOneOf(circumstance.Members.Select(member => member.Id));

            return circumstance;
        }
    }
}
