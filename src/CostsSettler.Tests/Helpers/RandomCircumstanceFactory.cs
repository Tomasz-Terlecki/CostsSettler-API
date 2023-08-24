using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;
public class RandomCircumstanceFactory
{
    public Circumstance Create(Guid? id = null, CircumstanceStatus? status = null, ICollection<Charge>? charges = null)
    {
        id = id ?? Guid.NewGuid();
        
        return new Circumstance
        { 
            Id = (Guid) id,
            CircumstanceStatus = status ?? CircumstanceStatus.New,
            DateTime = DateTime.UtcNow,
            Description = "description" + id,
            TotalAmount = 100,
            Charges = charges
        };
    }

    public ICollection<Circumstance> CreateCollection(int count)
    {
        var list = new List<Circumstance>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Create());
        }

        return list;
    }
}
