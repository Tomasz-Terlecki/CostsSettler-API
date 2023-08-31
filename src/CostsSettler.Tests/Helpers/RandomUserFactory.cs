using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;

/// <summary>
/// Factory for random user.
/// </summary>
public class RandomUserFactory : RandomFactoryBase<User, UserAttributes>
{
    /// <summary>
    /// Creates random user for given attributes.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <param name="attributes">User attributes to apply.</param>
    /// <returns>New user object.</returns>
    protected override User CreateModel(Guid id, UserAttributes? attributes = null)
    {
        return new User
        { 
            Id = id,
            FirstName = attributes?.FirstName ?? "firstName" + id.ToString(),
            LastName = attributes?.LastName ?? "lastName" + id.ToString(),
            Email = attributes?.Email ?? "email" + id.ToString(),
            Username = attributes?.Username ?? "username" + id.ToString()
        };
    }
}

/// <summary>
/// User attributes for RandomUserFactory.
/// </summary>
public class UserAttributes
{
    /// <summary>
    /// User username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// User first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// User last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// User email address.
    /// </summary>
    public string? Email { get; set; }
}
