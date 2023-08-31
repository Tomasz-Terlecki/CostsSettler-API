using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Commands;

/// <summary>
/// Settles charge identified by Id.
/// </summary>
public class SettleChargeCommand : IRequest<bool>
{
    /// <summary>
    /// Id of charge to settle.
    /// </summary>
    public Guid ChargeId { get; set; }

    /// <summary>
    /// SettleChargeCommand handler.
    /// </summary>
    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IChargeRepository _repository;
        private readonly IIdentityService _identityService;
        private readonly ICircumstanceRepository _circumstanceRepository;

        /// <summary>
        /// Creates new SettleChargeCommandHandler instance.
        /// </summary>
        /// <param name="mediator">Mediator for getting charge data.</param>
        /// <param name="repository">Repository that manages charges data.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        /// <param name="circumstanceRepository">Repository that manages circumstances data.</param>
        public SettleChargeCommandHandler(IMediator mediator, IChargeRepository repository, IIdentityService identityService,
            ICircumstanceRepository circumstanceRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _identityService = identityService;
            _circumstanceRepository = circumstanceRepository;
        }

        /// <summary>
        /// Handles the SettleChargeCommand command. Sets charge as settled IChargeRepository, 
        /// updates charge's Circumstance status.
        /// </summary>
        /// <param name="request">SettleChargeCommand to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>'true' if updating charge and circumstance is succeded, otherwise 'false'.</returns>
        /// <exception cref="DomainLogicException"></exception>
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
            circumstance.FixCircumstanceStatus();

            return await _repository.UpdateAsync(charge) && await _circumstanceRepository.UpdateAsync(circumstance);
        }
    }
}
