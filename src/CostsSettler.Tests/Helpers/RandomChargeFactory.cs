using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;

public class RandomChargeFactory : RandomFactoryBase<Charge, ChargeAttributes>
{
    protected override Charge CreateModel(Guid id, ChargeAttributes? attributes = null)
    {
        var creditorId = attributes?.CreditorId ?? attributes?.Creditor?.Id ?? Guid.NewGuid();
        var debtorId = attributes?.DebtorId ?? attributes?.Debtor?.Id ?? Guid.NewGuid();
        
        return new Charge
        {
            Id = id,
            Amount = attributes?.Amount ?? 100,
            CreditorId = creditorId,
            DebtorId = debtorId,
            CircumstanceId = attributes?.CircumstanceId ?? Guid.NewGuid(),
            ChargeStatus = attributes?.ChargeStatus ?? ChargeStatus.New,
            Circumstance = attributes?.Circumstance ?? new Circumstance
            {
                DateTime = DateTime.Now
            },
            Creditor = attributes?.Creditor ?? new User { Id = creditorId },
            Debtor = attributes?.Debtor ?? new User { Id = debtorId }
        };
    }
}

public class ChargeAttributes 
{
    public decimal? Amount { get; set; }
    public Guid? CreditorId { get; set; }
    public User? Creditor { get; set; }
    public Guid? DebtorId { get; set; }
    public User? Debtor { get; set; }
    public Guid? CircumstanceId { get; set; }
    public ChargeStatus? ChargeStatus { get; set; }
    public Circumstance? Circumstance { get; set; }
}
