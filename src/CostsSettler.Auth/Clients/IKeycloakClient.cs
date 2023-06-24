using CostsSettler.Auth.Models;

namespace CostsSettler.Auth.Clients;
public interface IKeycloakClient
{
    Task<string?> GetAccessTokenAsync();
    Task<ICollection<KcUser>?> GetUsersAsync(string accessToken);
    Task<KcUser?> GetUserByIdAsync(Guid userId, string accessToken);
}
