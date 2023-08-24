using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;

public class RandomChargeFactory : RandomFactoryBase<Charge, ChargeAttributes>
{
    protected override Charge CreateModel(Guid id, ChargeAttributes? attributes = null)
    {
        return new Charge
        {
            Id = id,
            Amount = attributes?.Amount ?? 100,
            CreditorId = attributes?.CreditorId ?? Guid.NewGuid(),
            DebtorId = attributes?.DebtorId ?? Guid.NewGuid(),
            CircumstanceId = attributes?.CircumstanceId ?? Guid.NewGuid(),
            ChargeStatus = attributes?.ChargeStatus ?? ChargeStatus.New,
        };
    }
}

public class ChargeAttributes 
{
    public decimal? Amount { get; set; }
    public Guid? CreditorId { get; set; }
    public Guid? DebtorId { get; set; }
    public Guid? CircumstanceId { get; set; }
    public ChargeStatus? ChargeStatus { get; set; }
}
