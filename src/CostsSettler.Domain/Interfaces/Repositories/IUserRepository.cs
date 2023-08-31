using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Interfaces.Repositories;

/// <summary>
/// Interface of repository that manages users data.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets all users from database.
    /// </summary>
    /// <returns>Collection of users.</returns>
    Task<ICollection<User>> GetAllAsync();

    /// <summary>
    /// Gets user by id.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>User with given id.</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Checks if user with given id exists in database.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>'true' id user with given id exists, otherwise 'false'.</returns>
    Task<bool> ExistsAsync(Guid id);
}
