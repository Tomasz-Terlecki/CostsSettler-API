using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;
public class CircumstanceRepository : RepositoryBase<Circumstance>, ICircumstanceRepository
{
    public CircumstanceRepository(CostsSettlerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<Circumstance>> GetByParamsAsync(GetCircumstancesByParamsQuery parameters)
    {
        var query = _dbContext.Circumstances.AsQueryable();

        if (parameters.UserId != Guid.Empty)
            query = query.Where(circumstance =>
                circumstance.Charges != null && circumstance.Charges.Any(charge => 
                    charge.DebtorId == parameters.UserId || charge.CreditorId == parameters.UserId));

        return await query.Include(circumstance => circumstance.Charges).ToListAsync();
    }

    public override Task<bool> AddAsync(Circumstance circumstance)
    {
        circumstance.FixCircumstanceStatus();
        
        return base.AddAsync(circumstance);
    }
    
    public override Task<bool> UpdateAsync(Circumstance circumstance)
    {
        circumstance.FixCircumstanceStatus();
        
        return base.UpdateAsync(circumstance);
    }
}
