using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class SettleChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }

    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IChargeRepository _repository;
        private readonly ICircumstanceRepository _circumstanceRepository;

        public SettleChargeCommandHandler(IChargeRepository repository, ICircumstanceRepository circumstanceRepository)
        {
            _repository = repository;
            _circumstanceRepository = circumstanceRepository;
        }

        public async Task<bool> Handle(SettleChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _repository.GetByIdAsync(request.ChargeId);

            // TODO: check if logged user is creditor
            if (charge is null)
                return false;

            charge.ChargeStatus = ChargeStatus.Settled;

            var circumstance = await _circumstanceRepository.GetByIdAsync(charge.CircumstanceId, new string[] { nameof(Circumstance.Charges) });

            return await _repository.UpdateAsync(charge);
        }
    }
}
