using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Queries;
public class GetChargeByIdQuery : IRequest<Charge>
{
    public Guid Id { get; set; }

    public GetChargeByIdQuery(Guid id)
    {
        Id = id;
    }

    public class GetChargeByIdQueryHandler : IRequestHandler<GetChargeByIdQuery, Charge>
    {
        private readonly IChargeRepository _repository;
        private readonly IUserRepository _userRepository;

        public GetChargeByIdQueryHandler(IChargeRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Charge> Handle(GetChargeByIdQuery request, CancellationToken cancellationToken)
        {
            Charge charge = await _repository.GetByIdAsync(request.Id, new string[] { nameof(Charge.Circumstance) })
                ?? throw new ObjectNotFoundException(typeof(Charge), request.Id);

            charge.Creditor = await _userRepository.GetByIdAsync(charge.CreditorId)
                ?? throw new ObjectNotFoundException(typeof(User), charge.CreditorId);

            charge.Debtor = await _userRepository.GetByIdAsync(charge.DebtorId)
                ?? throw new ObjectNotFoundException(typeof(User), charge.DebtorId);
            
            return charge;
        }
    }
}
