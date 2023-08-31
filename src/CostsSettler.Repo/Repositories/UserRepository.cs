using AutoMapper;
using CostsSettler.Auth.Clients;
using CostsSettler.Auth.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;

namespace CostsSettler.Repo.Repositories;

/// <summary>
/// Implementation of IUserRepository interface.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IKeycloakClient _keycloakClient;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates new UserRepository instance.
    /// </summary>
    /// <param name="keycloakClient">Service used to manage users data rfrom Keycloak.</param>
    /// <param name="mapper">Registered AutoMapper.</param>
    public UserRepository(IKeycloakClient keycloakClient, IMapper mapper)
    {
        _keycloakClient = keycloakClient;
        _mapper = mapper;
    }

    /// <summary>
    /// Checks if user with given id exists in database.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>'true' id user with given id exists, otherwise 'false'.</returns>
    public async Task<bool> ExistsAsync(Guid id)
    {
        var user = await _keycloakClient.GetUserByIdAsync(id, await GetAccessToken());

        return user is not null && user.Id != Guid.Empty;
    }

    /// <summary>
    /// Gets all users from database.
    /// </summary>
    /// <returns>Collection of users.</returns>
    public async Task<ICollection<User>> GetAllAsync()
    {
        var users = await _keycloakClient.GetUsersAsync(await GetAccessToken());

        return _mapper.Map<ICollection<User>>(users) ?? new List<User>();
    }

    /// <summary>
    /// Gets user by id.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>User with given id.</returns>
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _keycloakClient.GetUserByIdAsync(id, await GetAccessToken());

        return _mapper.Map<User>(user);
    }

    private async Task<string> GetAccessToken()
    {
        var accessToken = await _keycloakClient.GetAccessTokenAsync();

        return accessToken ?? throw new NullAccessTokenException();
    }
}
