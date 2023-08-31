using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;

/// <summary>
/// Implementation of IRepositoryBase<TModel> interface.
/// </summary>
/// <typeparam name="TModel">Domain model.</typeparam>
public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel>
    where TModel : ModelBase
{
    protected readonly CostsSettlerDbContext _dbContext;

    /// <summary>
    /// Creates new RepositoryBase.
    /// </summary>
    /// <param name="dbContext">DbContext used to manage TModel data.</param>
    public RepositoryBase(CostsSettlerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets TModel by id. Includes all properties that names are listen in 'includes'.
    /// </summary>
    /// <param name="id">TModel's identifier.</param>
    /// <param name="includes">List of properties to include during get.</param>
    /// <returns>TModel object with given id.</returns>
    public virtual async Task<TModel?> GetByIdAsync(Guid id, string[]? includes = null)
    {
        var query = _dbContext.Set<TModel>().AsQueryable();

        if (includes is not null)
            foreach (var item in includes)
                query = query.Include(item);

        return await query.SingleOrDefaultAsync(model => model.Id == id);
    }

    /// <summary>
    /// Adds new TModel object to database.
    /// </summary>
    /// <param name="model">TModel object to add.</param>
    /// <returns>'true' if adding succeded, otherwise 'false'.</returns>
    public virtual async Task<bool> AddAsync(TModel model)
    {
        var result = await _dbContext.Set<TModel>().AddAsync(model);

        var saveChangesResult = await _dbContext.SaveChangesAsync();

        return saveChangesResult > 0;
    }

    /// <summary>
    /// Updates existing model.
    /// </summary>
    /// <param name="model">TModel to update.</param>
    /// <returns>'true' if updating succeded, otherwise 'false'.</returns>
    public virtual async Task<bool> UpdateAsync(TModel model)
    {
        var result = _dbContext.Set<TModel>().Update(model);

        await _dbContext.SaveChangesAsync();

        return result.State == EntityState.Unchanged || result.State == EntityState.Modified;
    }
}
