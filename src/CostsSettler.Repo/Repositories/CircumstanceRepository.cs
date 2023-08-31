using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;

/// <summary>
/// Implementation of ICircumstanceRepository interface.
/// </summary>
public class CircumstanceRepository : RepositoryBase<Circumstance>, ICircumstanceRepository
{
    /// <summary>
    /// Creates new CircumstanceRepository that uses DbContext.
    /// </summary>
    /// <param name="dbContext">DbContext used to manage circumstances data.</param>
    public CircumstanceRepository(CostsSettlerDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Gets circumstances by parameters. Includes charges.
    /// </summary>
    /// <param name="parameters">Circumstances filters.</param>
    /// <returns>Circumstances that match given filters.</returns>
    public async Task<ICollection<Circumstance>> GetByParamsAsync(GetCircumstancesByParamsQuery parameters)
    {
        var query = _dbContext.Circumstances.AsQueryable();

        if (parameters.UserId != Guid.Empty)
            query = query.Where(circumstance =>
                circumstance.Charges != null && circumstance.Charges.Any(charge => 
                    charge.DebtorId == parameters.UserId || charge.CreditorId == parameters.UserId));

        return await query.Include(circumstance => circumstance.Charges).ToListAsync();
    }
}
