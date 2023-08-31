using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Interfaces.Repositories;

/// <summary>
/// Interface of repository that manages domain data.
/// Generic parameter TModel is a domain model.
/// </summary>
public interface IRepositoryBase<TModel> where TModel : ModelBase
{
    /// <summary>
    /// Gets TModel object by id.
    /// </summary>
    /// <param name="id">TModel's identifier.</param>
    /// <param name="includes">List of properties to include during get.</param>
    /// <returns>TModel object with given id.</returns>
    Task<TModel?> GetByIdAsync(Guid id, string[]? includes = null);
    
    /// <summary>
    /// Adds new TModel object to database.
    /// </summary>
    /// <param name="model">TModel object to add.</param>
    /// <returns>'true' if adding succeded, otherwise 'false'.</returns>
    Task<bool> AddAsync(TModel model);

    /// <summary>
    /// Updates existing model.
    /// </summary>
    /// <param name="model">TModel to update.</param>
    /// <returns>'true' if updating succeded, otherwise 'false'.</returns>
    Task<bool> UpdateAsync(TModel model);
}
