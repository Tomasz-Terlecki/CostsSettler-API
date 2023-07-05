using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class SettleChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }

    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IChargeRepository _repository;
        
        public SettleChargeCommandHandler(IMediator mediator, IChargeRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<bool> Handle(SettleChargeCommand request, CancellationToken cancellationToken)
        {
            Charge charge = await _mediator.Send(new GetChargeByIdQuery(request.ChargeId));
            Circumstance circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(charge.CircumstanceId));

            // TODO: check if logged user is creditor

            if (charge.ChargeStatus != ChargeStatus.Accepted ||
                    (circumstance.CircumstanceStatus != CircumstanceStatus.Accepted && 
                    circumstance.CircumstanceStatus != CircumstanceStatus.PartiallySettled))
                throw new DomainLogicException($"Could not settle charge of id {request.ChargeId}");

            
            charge.ChargeStatus = ChargeStatus.Settled;

            return await _repository.UpdateAsync(charge);
        }
    }
}
