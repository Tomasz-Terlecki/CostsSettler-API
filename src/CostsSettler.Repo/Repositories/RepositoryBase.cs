using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CostsSettler.Repo.Repositories;
public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel>
    where TModel : ModelBase
{
    protected readonly CostsSettlerDbContext _dbContext;

    public RepositoryBase(CostsSettlerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<ICollection<TModel>> GetAllAsync(string[]? includes = null)
    {
        var query = _dbContext.Set<TModel>().AsQueryable();

        if (includes is not null)
            foreach (var item in includes)
                query = query.Include(item);

        return await query.ToListAsync();
    }

    public virtual async Task<TModel?> GetByIdAsync(Guid id, string[]? includes = null)
    {
        var query = _dbContext.Set<TModel>().AsQueryable();

        if (includes is not null)
            foreach (var item in includes)
                query = query.Include(item);

        return await query.SingleOrDefaultAsync(model => model.Id == id);
    }

    public virtual async Task<bool> AddAsync(TModel model)
    {
        var result = await _dbContext.Set<TModel>().AddAsync(model);

        var saveChangesResult = await _dbContext.SaveChangesAsync();

        return saveChangesResult > 0;
    }

    public virtual async Task<bool> UpdateAsync(TModel model)
    {
        var result = _dbContext.Set<TModel>().Update(model);

        await _dbContext.SaveChangesAsync();

        return result.State == EntityState.Unchanged || result.State == EntityState.Modified;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var model = await GetByIdAsync(id);

        if (model == null)
        {
            throw new KeyNotFoundException($"Could not find {nameof(TModel)} of id {id}");
        }

        var result = _dbContext.Set<TModel>().Remove(model);

        await _dbContext.SaveChangesAsync();

        return result.State == EntityState.Deleted;
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Set<TModel>().AnyAsync(x => x.Id == id);
    }
}
