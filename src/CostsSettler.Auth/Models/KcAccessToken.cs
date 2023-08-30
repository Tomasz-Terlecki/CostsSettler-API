namespace CostsSettler.Auth.Models;

/// <summary>
/// Keycloak access token representation.
/// </summary>
public class KcAccessToken
{
    /// <summary>
    /// Actual access token as string.
    /// </summary>
    public string AccessToken { get; set; } = null!;
}
