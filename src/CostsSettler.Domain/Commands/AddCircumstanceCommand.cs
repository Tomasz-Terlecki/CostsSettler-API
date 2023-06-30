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
            if (request.DebtorsIds.Contains(request.CreditorId))
                throw new ObjectReferenceException($"The creditor cannot be added as debtor");

            foreach (var userId in request.DebtorsIds)
                if (userId == Guid.Empty || !(await _userRepository.ExistsAsync(userId)))
                    throw new ObjectReferenceException($"The use of id {userId} does not exist");

            if (request.CreditorId == Guid.Empty || !(await _userRepository.ExistsAsync(request.CreditorId)))
                throw new ObjectReferenceException($"The use of id {request.CreditorId} does not exist");

            var membersCount = request.DebtorsIds.Count + 1;

            var debtors = request.DebtorsIds
                .Select(member => new MemberCharge
                {
                    UserId = member,
                    CircumstanceRole = CircumstanceRole.Debtor,
                    ChargeStatus = ChargeStatus.New,
                    Amount = Round(request.TotalAmount / membersCount)
                }).ToList();

            var members = debtors.Append(new MemberCharge
            {
                UserId = request.CreditorId,
                CircumstanceRole = CircumstanceRole.Creditor,
                ChargeStatus = ChargeStatus.New,
                Amount = Round(request.TotalAmount / membersCount)
            }).ToList();

            var circumstance = new Circumstance
            {
                Description = request.Description,
                TotalAmount = request.TotalAmount,
                CircumstanceStatus = CircumstanceStatus.New,
                Members = members
            };
            
            return await _circumstanceRepository.AddAsync(circumstance);
        }

        private static decimal Round(decimal number)
        {
            return Math.Round(number, 2, MidpointRounding.ToPositiveInfinity);
        }
    }
}
