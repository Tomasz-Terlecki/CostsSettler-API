using CostsSettler.Domain.Models;
using CostsSettler.Domain.Queries;

namespace CostsSettler.Domain.Interfaces.Repositories;

/// <summary>
/// Interface of repository that manages circumstances data.
/// </summary>
public interface ICircumstanceRepository : IRepositoryBase<Circumstance>
{
    /// <summary>
    /// Gets circumstances by parameters.
    /// </summary>
    /// <param name="parameters">Circumstances filters.</param>
    /// <returns></returns>
    Task<ICollection<Circumstance>> GetByParamsAsync(GetCircumstancesByParamsQuery parameters);
}
