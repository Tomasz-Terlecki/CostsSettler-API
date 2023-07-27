using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class SettleChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }

    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IChargeRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ICircumstanceRepository _circumstanceRepository;

        public SettleChargeCommandHandler(IMediator mediator, IChargeRepository repository, IIdentityService identityService,
            ICircumstanceRepository circumstanceRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _identityService = identityService;
            _circumstanceRepository = circumstanceRepository;
        }

        public async Task<bool> Handle(SettleChargeCommand request, CancellationToken cancellationToken)
        {
            Charge charge = await _mediator.Send(new GetChargeByIdQuery(request.ChargeId));
            Circumstance circumstance = await _mediator.Send(new GetCircumstanceByIdQuery(charge.CircumstanceId));

            _identityService.CheckEqualityWithLoggedUserId(charge.CreditorId);

            if (charge.ChargeStatus != ChargeStatus.Accepted ||
                    (circumstance.CircumstanceStatus != CircumstanceStatus.Accepted && 
                    circumstance.CircumstanceStatus != CircumstanceStatus.PartiallySettled))
                throw new DomainLogicException($"Could not settle charge of id {request.ChargeId}");

            
            charge.ChargeStatus = ChargeStatus.Settled;

            return await _repository.UpdateAsync(charge) && await _circumstanceRepository.UpdateAsync(circumstance);
        }
    }
}
