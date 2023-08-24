using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;
public class RandomUserFactory : RandomFactoryBase<User, UserAttributes>
{
    protected override User CreateModel(Guid id, UserAttributes? attributes = null)
    {
        return new User
        { 
            Id = id,
            FirstName = attributes?.FirstName ?? "firstName" + id.ToString(),
            LastName = attributes?.LastName ?? "lastName" + id.ToString(),
            Email = attributes?.Email ?? "email" + id.ToString(),
            Username = attributes?.Username ?? "username" + id.ToString(),
            Charges = attributes?.Charges ?? new List<Charge>()
        };
    }
}

public class UserAttributes
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public ICollection<Charge>? Charges { get; set; }
}
