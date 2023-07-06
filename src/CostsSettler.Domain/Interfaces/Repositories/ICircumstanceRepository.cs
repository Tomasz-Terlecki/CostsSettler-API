using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;

namespace CostsSettler.Domain.Interfaces.Repositories;
public interface ICircumstanceRepository : IRepositoryBase<Circumstance>
{
    Task<ICollection<Circumstance>> GetByParamsAsync(GetCircumstancesByParamsQuery parameters);
}
