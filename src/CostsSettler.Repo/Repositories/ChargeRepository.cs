using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;
public class ChargeRepository : RepositoryBase<Charge>, IChargeRepository
{
    public ChargeRepository(CostsSettlerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters)
    {
        var query = _dbContext.Charges.AsQueryable();

        if (parameters.DebtorId != Guid.Empty)
            query = query.Where(charge => charge.DebtorId == parameters.DebtorId);

        if (parameters.CreditorId != Guid.Empty)
            query = query.Where(charge => charge.CreditorId == parameters.CreditorId);

        if (parameters.ChargeStatus != ChargeStatus.None)
            query = query.Where(charge => charge.ChargeStatus == parameters.ChargeStatus);

        return await query.Include(charge => charge.Circumstance).ToListAsync();
    }
}
