using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Models;

/// <summary>
/// Domain model representing charge.
/// </summary>
public class Charge : ModelBase
{
    /// <summary>
    /// Creditor of charge.
    /// </summary>
    public User Creditor { get; set; } = null!;

    /// <summary>
    /// Creditor identifier.
    /// </summary>
    public Guid CreditorId { get; set; }
    
    /// <summary>
    /// Debtor of charge.
    /// </summary>
    public User Debtor { get; set; } = null!;
    
    /// <summary>
    /// Debtor identifier.
    /// </summary>
    public Guid DebtorId { get; set; }
    
    /// <summary>
    /// Circumstance the charge depends to.
    /// </summary>
    public Circumstance Circumstance { get; set; } = null!;
    
    /// <summary>
    /// Circumstance identifier.
    /// </summary>
    public Guid CircumstanceId { get; set; }
    
    /// <summary>
    /// Amount of charge.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Status of charge.
    /// </summary>
    public ChargeStatus ChargeStatus { get; set; }

    /// <summary>
    /// Circumstance's date and time.
    /// </summary>
    public DateTime DateTime { get => Circumstance.DateTime; }
}
