using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;

namespace CostsSettler.Repo.Repositories;
public class CircumstanceRepository : RepositoryBase<Circumstance>, ICircumstanceRepository
{
    public CircumstanceRepository(CostsSettlerDbContext dbContext) : base(dbContext)
    {
    }
}
