using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Extensions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Services;
using MediatR;

namespace CostsSettler.Domain.Commands;

/// <summary>
/// Adds new circumstance.
/// </summary>
public class AddCircumstanceCommand : IRequest<bool>
{
    /// <summary>
    /// Circumstance description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Circumstance amount.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// List of debtors ids.
    /// </summary>
    public ICollection<Guid> DebtorsIds { get; set; } = null!;
    
    /// <summary>
    /// Creditor id.
    /// </summary>
    public Guid CreditorId { get; set; }
    
    /// <summary>
    /// Date of circumstance in format 'yyyy-mm-dd'.
    /// </summary>
    public string Date { get; set; } = null!;
    
    /// <summary>
    /// Time of circumstance in format 'hh:mm'.
    /// </summary>
    public string Time { get; set; } = null!;

    /// <summary>
    /// AddCircumstanceCommand handler.
    /// </summary>
    public class AddCircumstanceCommandHandler : IRequestHandler<AddCircumstanceCommand, bool>
    {
        private readonly ICircumstanceRepository _circumstanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Creates new AddCircumstanceCommandHandler instance.
        /// </summary>
        /// <param name="circumstanceRepository">Repository that manages circumstances data.</param>
        /// <param name="userRepository">Repository that manages users data.</param>
        /// <param name="identityService">Service that authorizes user that invokes the Handler.</param>
        public AddCircumstanceCommandHandler(ICircumstanceRepository circumstanceRepository, 
            IUserRepository userRepository, IIdentityService identityService)
        {
            _circumstanceRepository = circumstanceRepository;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the AddCircumstanceCommand command. Calculates circumstance properties and 
        /// associates new charges with calculated properties. 
        /// Adds circumstance and charges using given circumstance repositories.
        /// </summary>
        /// <param name="request">AddCircumstanceCommand to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>'true' if circumstance adding is succeded, otherwise 'false'.</returns>
        /// <exception cref="ObjectReferenceException"></exception>
        /// <exception cref="ObjectNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> Handle(AddCircumstanceCommand request, CancellationToken cancellationToken)
        {
            _identityService.CheckEqualityWithLoggedUserId(request.CreditorId);

            if (request.DebtorsIds.Contains(request.CreditorId))
                throw new ObjectReferenceException($"The creditor cannot be added as debtor");

            foreach (var debtorId in request.DebtorsIds)
                if (debtorId == Guid.Empty || !await _userRepository.ExistsAsync(debtorId))
                    throw new ObjectNotFoundException(typeof(User), debtorId);

            if (request.CreditorId == Guid.Empty || !await _userRepository.ExistsAsync(request.CreditorId))
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
            
            var date = request.Date.ToDateOnly() ?? throw new ArgumentException(nameof(request.Date));
            var time = request.Time.ToTimeOnly() ?? throw new ArgumentException(nameof(request.Time));

            var circumstance = new Circumstance
            {
                Description = request.Description,
                TotalAmount = request.TotalAmount,
                DateTime = date.ToDateTime(time),
                Charges = charges
            };
            
            circumstance.FixCircumstanceStatus();

            return await _circumstanceRepository.AddAsync(circumstance);
        }

        private static decimal Round(decimal number)
        {
            return Math.Round(number, 2, MidpointRounding.ToPositiveInfinity);
        }
    }
}
