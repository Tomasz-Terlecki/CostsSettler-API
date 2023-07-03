using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;
public class MemberChargeRepository : RepositoryBase<MemberCharge>, IMemberChargeRepository
{
    public MemberChargeRepository(CostsSettlerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<MemberCharge>> GetByParamsAsync(GetChargesByParamsQuery parameters)
    {
        var query = _dbContext.MemberCharges.AsQueryable();

        if (parameters.UserId != Guid.Empty)
            query = query.Where(charge => charge.UserId == parameters.UserId);

        if (parameters.ChargeStatus != ChargeStatus.None)
            query = query.Where(charge => charge.ChargeStatus == parameters.ChargeStatus);

        return await query.Include(charge => charge.Circumstance).ToListAsync();
    }
}
