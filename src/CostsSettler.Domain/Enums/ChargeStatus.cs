namespace CostsSettler.Domain.Enums;

/// <summary>
/// Status of charge.
/// </summary>
public enum ChargeStatus : byte
{
    /// <summary>
    /// Empty charge status.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Charge is new.
    /// </summary>
    New = 1,
    
    /// <summary>
    /// Charge is accepted.
    /// </summary>
    Accepted = 2,

    /// <summary>
    /// Charge is rejected.
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// Charge is settled.
    /// </summary>
    Settled = 4,
}
