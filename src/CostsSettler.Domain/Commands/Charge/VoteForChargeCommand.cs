using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Commands;

/// <summary>
/// Votes for charge.
/// </summary>
public class VoteForChargeCommand : IRequest<bool>
{
    /// <summary>
    /// Id of charge to vote.
    /// </summary>
    public Guid ChargeId { get; set; }

    /// <summary>
    /// Vote type.
    /// </summary>
    public ChargeVote ChargeVote { get; set; }
    
    /// <summary>
    /// VoteForChargeCommand handler.
    /// </summary>
    public class VoteForChargeCommandHandler : IRequestHandler<VoteForChargeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IChargeRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ICircumstanceRepository _circumstanceRepository;

        
        /// <summary>
        /// Creates new VoteForChargeCommandHandler instance.
        /// </summary>
        /// <param name="mediator">Mediator for getting charge data.</param>
        /// <param name="repository">Repository that manages charges data.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        /// <param name="circumstanceRepository">Repository that manages circumstances data.</param>
        public VoteForChargeCommandHandler(IMediator mediator, IChargeRepository repository, IIdentityService identityService,
            ICircumstanceRepository circumstanceRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _identityService = identityService;
            _circumstanceRepository = circumstanceRepository;
        }

        /// <summary>
        /// Handles the VoteForChargeCommand command. Chechs charge status and sets 
        /// the charge status conditionally, then updates charge's circumstance status.
        /// </summary>
        /// <param name="request">VoteForChargeCommand to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>'true' if updating charge and circumstance is succeded, otherwise 'false'.</returns>
        /// <exception cref="DomainLogicException"></exception>
        /// <exception cref="ObjectReferenceException"></exception>
        public async Task<bool> Handle(VoteForChargeCommand request, CancellationToken cancellationToken)
        {
            Charge charge = await _mediator.Send(new GetChargeByIdQuery(request.ChargeId));
            Circumstance circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(charge.CircumstanceId));

            _identityService.CheckEqualityWithLoggedUserId(charge.DebtorId);

            switch (request.ChargeVote)
            {
                case ChargeVote.Accept:
                {
                    if (charge.ChargeStatus == ChargeStatus.Settled ||
                            circumstance.CircumstanceStatus == CircumstanceStatus.PartiallySettled ||
                            circumstance.CircumstanceStatus == CircumstanceStatus.Settled)
                        throw new DomainLogicException($"Could not vote for charge with {ChargeVote.Accept}");
                    
                    charge.ChargeStatus = ChargeStatus.Accepted;
                    break;
                }
                case ChargeVote.Reject:
                {
                    if (charge.ChargeStatus == ChargeStatus.Settled ||
                            circumstance.CircumstanceStatus == CircumstanceStatus.PartiallySettled ||
                            circumstance.CircumstanceStatus == CircumstanceStatus.Settled)
                        throw new DomainLogicException($"Could not vote for charge with {ChargeVote.Reject}");

                    charge.ChargeStatus = ChargeStatus.Rejected;
                    break;
                }
                case ChargeVote.None:
                default:
                    throw new ObjectReferenceException($"Could not vote for charge, because charge vote was {request.ChargeVote}");
            }

            circumstance.FixCircumstanceStatus();

            return await _repository.UpdateAsync(charge) && await _circumstanceRepository.UpdateAsync(circumstance);
        }
    }
}
