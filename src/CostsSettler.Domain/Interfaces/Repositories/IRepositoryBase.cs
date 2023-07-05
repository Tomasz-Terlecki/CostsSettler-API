using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Interfaces.Repositories;
public interface IRepositoryBase<TModel> where TModel : ModelBase
{
    Task<ICollection<TModel>> GetAllAsync(string[]? includes = null);
    Task<TModel?> GetByIdAsync(Guid id, string[]? includes = null);
    Task<bool> AddAsync(TModel model);
    Task<bool> UpdateAsync(TModel model);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
