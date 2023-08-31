using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;

namespace CostsSettler.Domain.Interfaces.Repositories;

/// <summary>
/// Interface of repository that manages charges data.
/// </summary>
public interface IChargeRepository : IRepositoryBase<Charge>
{
    /// <summary>
    /// Gets charges by parameters.
    /// </summary>
    /// <param name="parameters">Charges filters.</param>
    /// <returns></returns>
    Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters);
}
