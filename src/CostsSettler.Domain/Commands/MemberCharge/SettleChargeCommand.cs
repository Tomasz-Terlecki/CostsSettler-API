using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class SettleChargeCommand : IRequest<bool>
{
    public Guid MemberChargeId { get; set; }

    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IMemberChargeRepository _repository;
        private readonly ICircumstanceRepository _circumstanceRepository;

        public SettleChargeCommandHandler(IMemberChargeRepository repository, ICircumstanceRepository circumstanceRepository)
        {
            _repository = repository;
            _circumstanceRepository = circumstanceRepository;
        }

        public async Task<bool> Handle(SettleChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _repository.GetByIdAsync(request.MemberChargeId);

            if (charge is null || charge.CircumstanceRole != CircumstanceRole.Creditor)
                return false;

            charge.ChargeStatus = ChargeStatus.Settled;

            var circumstance = await _circumstanceRepository.GetByIdAsync(charge.CircumstanceId, new string[] { nameof(Circumstance.Members) });

            if (circumstance is not null &&
                circumstance.Members is not null &&
                circumstance.Members.All(charge => charge.ChargeStatus == ChargeStatus.Settled))
            {
                circumstance.CircumstanceStatus = CircumstanceStatus.Settled;
                await _circumstanceRepository.UpdateAsync(circumstance);
            }

            return await _repository.UpdateAsync(charge);
        }
    }
}
