using AutoMapper;
using CostsSettler.Auth.Clients;
using CostsSettler.Auth.Exceptions;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Models;

namespace CostsSettler.Repo.Repositories;
public class UserRepository : IUserRepository
{
    private readonly IKeycloakClient _keycloakClient;
    private readonly IMapper _mapper;

    public UserRepository(IKeycloakClient keycloakClient, IMapper mapper)
    {
        _keycloakClient = keycloakClient;
        _mapper = mapper;
    }

    public Task<bool> AddAsync(User model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        var users = await _keycloakClient.GetUsersAsync(await GetAccessToken());

        return _mapper.Map<ICollection<User>>(users) ?? new List<User>();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _keycloakClient.GetUserByIdAsync(id, await GetAccessToken());

        return _mapper.Map<User>(user);
    }

    public Task<bool> UpdateAsync(User model)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GetAccessToken()
    {
        var accessToken = await _keycloakClient.GetAccessTokenAsync();

        return accessToken ?? throw new NullAccessTokenException();
    }
}
