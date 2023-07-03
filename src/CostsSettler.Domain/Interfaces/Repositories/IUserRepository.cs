using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Interfaces.Repositories;
public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<bool> AddAsync(User model);
    Task<bool> UpdateAsync(User model);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
