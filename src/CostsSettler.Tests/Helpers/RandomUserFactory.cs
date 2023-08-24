using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;
public class RandomUserFactory
{
    public User Create(Guid? id = null, ICollection<Charge>? charges = null)
    {
        id = id ?? Guid.NewGuid();
        
        return new User
        { 
            Id = (Guid) id,
            FirstName = "firstName" + id.ToString(),
            LastName = "lastName" + id.ToString(),
            Email = "email" + id.ToString(),
            Username = "username" + id.ToString(),
            Charges = charges
        };
    }

    public ICollection<User> CreateCollection(int count)
    {
        var list = new List<User>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Create());
        }

        return list;
    }
}
