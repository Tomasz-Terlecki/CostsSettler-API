namespace CostsSettler.Auth.Config;

/// <summary>
/// Configuration of KeycloakClient.
/// </summary>
public class KeycloakClientConfig
{
    /// <summary>
    /// Realm name.
    /// </summary>
    public string Realm { get; set; } = null!;
    
    /// <summary>
    /// Keycloak server base url.
    /// </summary>
    public string AuthServerUrl { get; set; } = null!;
    
    /// <summary>
    /// Keycloak client secret.
    /// </summary>
    public string Secret { get; set; } = null!;

    /// <summary>
    /// Keycloak client-id (not Guid for client).
    /// </summary>
    public string ClientId { get; set; } = null!;
}
