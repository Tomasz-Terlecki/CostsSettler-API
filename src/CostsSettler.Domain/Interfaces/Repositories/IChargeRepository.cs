using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;

namespace CostsSettler.Domain.Interfaces.Repositories;
public interface IChargeRepository : IRepositoryBase<Charge>
{
    Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters);
}
