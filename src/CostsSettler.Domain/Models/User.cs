namespace CostsSettler.Domain.Models;

/// <summary>
/// Domain model representing user.
/// </summary>
public class User : ModelBase
{
    /// <summary>
    /// User's username.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// First name of user.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of user.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; } = null!;
}
