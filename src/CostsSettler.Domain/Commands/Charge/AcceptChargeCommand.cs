using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class AcceptChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }

    public class AcceptChargeCommandHandler : IRequestHandler<AcceptChargeCommand, bool>
    {
        private readonly IChargeRepository _repository;
        private readonly ICircumstanceRepository _circumstanceRepository;

        public AcceptChargeCommandHandler(IChargeRepository repository, ICircumstanceRepository circumstanceRepository)
        {
            _repository = repository;
            _circumstanceRepository = circumstanceRepository;
        }

        public async Task<bool> Handle(AcceptChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _repository.GetByIdAsync(request.ChargeId);

            // TODO: check if logged user is debtor
            if (charge is null)
                return false;

            charge.ChargeStatus = ChargeStatus.Accepted;

            var circumstance = await _circumstanceRepository.GetByIdAsync(charge.CircumstanceId, new string[] { nameof(Circumstance.Charges) });

            if (circumstance is not null)
            {
                circumstance.CircumstanceStatus = 
                    circumstance.Charges is not null && 
                    circumstance.Charges.All(charge => charge.ChargeStatus == ChargeStatus.Accepted)
                        ? CircumstanceStatus.Accepted
                        : CircumstanceStatus.PartiallyAccepted;

                await _circumstanceRepository.UpdateAsync(circumstance);
            }

            return await _repository.UpdateAsync(charge);
        }
    }
}
