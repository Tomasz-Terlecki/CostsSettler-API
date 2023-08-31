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
    /// <returns>Charges that match given filters.</returns>
    Task<ICollection<Charge>> GetByParamsAsync(GetChargesByParamsQuery parameters);
}
