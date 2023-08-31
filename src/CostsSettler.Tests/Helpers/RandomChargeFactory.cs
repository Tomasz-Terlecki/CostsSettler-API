using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;

/// <summary>
/// Factory for random charge.
/// </summary>
public class RandomChargeFactory : RandomFactoryBase<Charge, ChargeAttributes>
{
    /// <summary>
    /// Creates random charge for given attributes.
    /// </summary>
    /// <param name="id">Charge id.</param>
    /// <param name="attributes">Charge attributes to apply.</param>
    /// <returns>New charge object.</returns>
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

/// <summary>
/// Charge attributes for RandomChargeFactory.
/// </summary>
public class ChargeAttributes 
{
    /// <summary>
    /// Charge amount.
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Charge creditor id.
    /// </summary>
    public Guid? CreditorId { get; set; }

    /// <summary>
    /// Charge creditor.
    /// </summary>
    public User? Creditor { get; set; }

    /// <summary>
    /// Charge debtor id.
    /// </summary>
    public Guid? DebtorId { get; set; }
    
    /// <summary>
    /// Charge debtor.
    /// </summary>
    public User? Debtor { get; set; }

    /// <summary>
    /// Charge circumstance id.
    /// </summary>
    public Guid? CircumstanceId { get; set; }

    /// <summary>
    /// Charge status.
    /// </summary>
    public ChargeStatus? ChargeStatus { get; set; }
    
    /// <summary>
    /// Charge circumstance.
    /// </summary>
    public Circumstance? Circumstance { get; set; }
}
