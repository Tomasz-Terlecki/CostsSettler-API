using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using CostsSettler.Domain.Extensions;

namespace CostsSettler.Repo.Repositories;
public class ChargeRepository : RepositoryBase<Charge>, IChargeRepository
{
    private readonly IUserRepository _userRepository;

    public ChargeRepository(CostsSettlerDbContext dbContext, IUserRepository userRepository) : base(dbContext)
    {
        _userRepository = userRepository;
    }

    public async Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters)
    {
        var query = _dbContext.Charges.AsQueryable();

        if (parameters.UserId != Guid.Empty)
            query = query.Where(
                charge => charge.DebtorId == parameters.UserId ||
                charge.CreditorId == parameters.UserId);

        if (parameters.ChargeStatus != ChargeStatus.None)
            query = query.Where(charge => charge.ChargeStatus == parameters.ChargeStatus);

        var dateFrom = parameters.DateFrom?.ToDateOnly()?.ToDateTime(TimeOnly.MinValue).Date;
        var dateTo = parameters.DateTo?.ToDateOnly()?.ToDateTime(TimeOnly.MinValue).Date;
        
        if (dateFrom is not null && dateTo is not null)
        {
            query = query.Where(charge => dateFrom <= charge.Circumstance.DateTime.Date &&
                                charge.Circumstance.DateTime.Date <= dateTo);
        }

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
