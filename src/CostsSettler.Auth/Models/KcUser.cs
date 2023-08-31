namespace CostsSettler.Auth.Models;

/// <summary>
/// Keycloak user implementation.
/// </summary>
public class KcUser
{
    /// <summary>
    /// Keycloak user identifier.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Keycloak user email address.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Keycloak user first name.
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Keycloak user last name.
    /// </summary>
    public string? LastName { get; set; }
}
