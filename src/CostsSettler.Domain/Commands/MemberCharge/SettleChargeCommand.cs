using CostsSettler.Domain.Interfaces.Repositories;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class SettleChargeCommand : IRequest<bool>
{
    public Guid MemberChargeId { get; set; }

    public class SettleChargeCommandHandler : IRequestHandler<SettleChargeCommand, bool>
    {
        private readonly IMemberChargeRepository _repository;

        public SettleChargeCommandHandler(IMemberChargeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(SettleChargeCommand request, CancellationToken cancellationToken)
        {
            var charge = await _repository.GetByIdAsync(request.MemberChargeId);

            if (charge is null)
                return false;

            await _repository.UpdateAsync(charge);

            return true;
        }
    }
}
