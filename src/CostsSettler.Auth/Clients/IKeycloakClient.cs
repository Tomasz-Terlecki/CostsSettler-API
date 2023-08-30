using CostsSettler.Auth.Models;

namespace CostsSettler.Auth.Clients;

/// <summary>
/// Client for managing Keycloak tool.
/// </summary>
public interface IKeycloakClient
{
    /// <summary>
    /// Generates access token for Keycloak client.
    /// </summary>
    /// <returns>Generated access token as string value.</returns>
    Task<string?> GetAccessTokenAsync();
    
    /// <summary>
    /// Gets all users for registered Keycloak realm.
    /// </summary>
    /// <param name="accessToken">Access token to authenticate application.</param>
    /// <returns>Collection of Keycloak users returned.</returns>
    Task<ICollection<KcUser>?> GetUsersAsync(string accessToken);
    
    /// <summary>
    /// Gets user by given Keycloak user identifier.
    /// </summary>
    /// <param name="userId">Identifier of user in Keycloak.</param>
    /// <param name="accessToken">Access token to authenticate application.</param>
    /// <returns></returns>
    Task<KcUser?> GetUserByIdAsync(Guid userId, string accessToken);
}
