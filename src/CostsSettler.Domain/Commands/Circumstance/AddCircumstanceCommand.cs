using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using MediatR;

namespace CostsSettler.Domain.Commands;
public class AddCircumstanceCommand : IRequest<bool>
{
    public string Description { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ICollection<Guid> DebtorsIds { get; set; } = null!;
    public Guid CreditorId { get; set; }

    public class AddCircumstanceCommandHandler : IRequestHandler<AddCircumstanceCommand, bool>
    {
        private readonly ICircumstanceRepository _circumstanceRepository;
        private readonly IUserRepository _userRepository;

        public AddCircumstanceCommandHandler(ICircumstanceRepository circumstanceRepository, IUserRepository userRepository)
        {
            _circumstanceRepository = circumstanceRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(AddCircumstanceCommand request, CancellationToken cancellationToken)
        {
            // TODO: check if logged user is creditor

            if (request.DebtorsIds.Contains(request.CreditorId))
                throw new ObjectReferenceException($"The creditor cannot be added as debtor");

            foreach (var debtorId in request.DebtorsIds)
                if (debtorId == Guid.Empty || !(await _userRepository.ExistsAsync(debtorId)))
                    throw new ObjectNotFoundException(typeof(User), debtorId);

            if (request.CreditorId == Guid.Empty || !(await _userRepository.ExistsAsync(request.CreditorId)))
                throw new ObjectNotFoundException(typeof(User), request.CreditorId);

            var membersCount = request.DebtorsIds.Count + 1;

            var charges = request.DebtorsIds
                .Select(debtorId => new Charge
                {
                    DebtorId = debtorId,
                    CreditorId = request.CreditorId,
                    ChargeStatus = ChargeStatus.New,
                    Amount = Round(request.TotalAmount / membersCount)
                }).ToList();

            var circumstance = new Circumstance
            {
                Description = request.Description,
                TotalAmount = request.TotalAmount,
                Charges = charges
            };
            
            return await _circumstanceRepository.AddAsync(circumstance);
        }

        private static decimal Round(decimal number)
        {
            return Math.Round(number, 2, MidpointRounding.ToPositiveInfinity);
        }
    }
}
