using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class VoteForChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }
    public ChargeVote ChargeVote { get; set; }
    
    public class VoteForChargeCommandHandler : IRequestHandler<VoteForChargeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IChargeRepository _repository;
        
        public VoteForChargeCommandHandler(IMediator mediator, IChargeRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<bool> Handle(VoteForChargeCommand request, CancellationToken cancellationToken)
        {
            Charge charge = await _mediator.Send(new GetChargeByIdQuery(request.ChargeId));
            Circumstance circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(charge.CircumstanceId));

            // TODO: check if logged user is debtor

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

            return await _repository.UpdateAsync(charge);
        }
    }
}
