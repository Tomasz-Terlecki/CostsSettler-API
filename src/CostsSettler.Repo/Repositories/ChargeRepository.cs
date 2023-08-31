using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using CostsSettler.Domain.Extensions;

namespace CostsSettler.Repo.Repositories;

/// <summary>
/// Implementation of IChargeRepository interface.
/// </summary>
public class ChargeRepository : RepositoryBase<Charge>, IChargeRepository
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Creates new ChargeRepository that uses DbContext.
    /// </summary>
    /// <param name="dbContext">DbContext used to manage charges data.</param>
    /// <param name="userRepository">Repository that manages users data.</param>
    public ChargeRepository(CostsSettlerDbContext dbContext, IUserRepository userRepository) : base(dbContext)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Gets charges by parameters. Includes circumstance, debtors and creditors.
    /// </summary>
    /// <param name="parameters">Charges filters.</param>
    /// <returns>Charges that match given filters.</returns>
    /// <exception cref="ObjectNotFoundException"></exception>
    public async Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters)
    {
        var query = _dbContext.Charges.AsQueryable();

        if (parameters.UserId != Guid.Empty)
            query = query.Where(
                charge => charge.DebtorId == parameters.UserId ||
                charge.CreditorId == parameters.UserId);

        var dateFrom = parameters.DateFrom?.ToDateOnly()?.ToDateTime(TimeOnly.MinValue).Date;
        var dateTo = parameters.DateTo?.ToDateOnly()?.ToDateTime(TimeOnly.MinValue).Date;

        if (dateFrom is not null && dateTo is not null)
            query = query.Where(charge => dateFrom <= charge.Circumstance.DateTime.Date &&
                                charge.Circumstance.DateTime.Date <= dateTo);

        if (!string.IsNullOrWhiteSpace(parameters.CircumstanceDescription))
            query = query.Where(charge => 
                charge.Circumstance.Description
                    .Contains(parameters.CircumstanceDescription));

        if (parameters.AmountFrom is not null)
            query = query.Where(charge => charge.Amount >= parameters.AmountFrom);

        if (parameters.AmountTo is not null)
            query = query.Where(charge => charge.Amount <= parameters.AmountTo);

        var charges = await query.Include(charge => charge.Circumstance).ToListAsync();

        foreach (var charge in charges)
        {
            charge.Creditor = await _userRepository.GetByIdAsync(charge.CreditorId) 
                ?? throw new ObjectNotFoundException(charge.Creditor.GetType(), charge.CreditorId);
            charge.Debtor = await _userRepository.GetByIdAsync(charge.DebtorId)
                ?? throw new ObjectNotFoundException(charge.Debtor.GetType(), charge.DebtorId);
        }

        return charges;
    }
}
