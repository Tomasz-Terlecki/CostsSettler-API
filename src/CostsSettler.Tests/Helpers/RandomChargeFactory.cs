using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;
public class RandomChargeFactory
{
    public Charge Create(Guid? id = null, ChargeStatus? status = null,
        Guid? debtorId = null, Guid? creditorId = null, Guid? circumstanceId = null)
    {
        id = id ?? Guid.NewGuid();

        return new Charge
        {
            Id = (Guid)id,
            ChargeStatus = status ?? ChargeStatus.New,
            Amount = 100,
            CircumstanceId = circumstanceId ?? Guid.NewGuid(),
            CreditorId = creditorId ?? Guid.NewGuid(),
            DebtorId = debtorId ?? Guid.NewGuid()
        };
    }

    public ICollection<Charge> CreateCollection(int count)
    {
        var list = new List<Charge>();

        for (int i = 0; i < count; i++)
        {
            list.Add(Create());
        }

        return list;
    }
}
