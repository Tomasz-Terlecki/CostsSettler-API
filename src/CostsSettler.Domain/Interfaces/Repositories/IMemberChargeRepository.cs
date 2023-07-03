using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;

namespace CostsSettler.Domain.Interfaces.Repositories;
public interface IMemberChargeRepository : IRepositoryBase<MemberCharge>
{
    Task<ICollection<MemberCharge>> GetByParamsAsync(GetChargesByParamsQuery parameters);
}
