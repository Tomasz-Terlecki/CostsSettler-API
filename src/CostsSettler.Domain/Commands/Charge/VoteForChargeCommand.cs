using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class VoteForChargeCommand : IRequest<bool>
{
    public Guid ChargeId { get; set; }
    public ChargeVote ChargeVote { get; set; }
    
    public class VoteForChargeCommandHandler : IRequestHandler<VoteForChargeCommand, bool>
    {
        private readonly IChargeRepository _repository;
        private readonly ICircumstanceRepository _circumstanceRepository;

        public VoteForChargeCommandHandler(IChargeRepository repository, ICircumstanceRepository circumstanceRepository)
        {
            _repository = repository;
            _circumstanceRepository = circumstanceRepository;
        }

        public async Task<bool> Handle(VoteForChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _repository.GetByIdAsync(request.ChargeId);

            // TODO: check if logged user is debtor
            if (charge is null)
                return false;

            charge.ChargeStatus = request.ChargeVote switch
            {
                ChargeVote.Reject => ChargeStatus.Rejected,
                ChargeVote.Accept => ChargeStatus.Accepted,
                _ => throw new ObjectReferenceException($"Could not vote for charge, because charge vote was {request.ChargeVote}")
            };

            var circumstance = await _circumstanceRepository.GetByIdAsync(charge.CircumstanceId, new string[] { nameof(Circumstance.Charges) });

            return await _repository.UpdateAsync(charge);
        }
    }
}
