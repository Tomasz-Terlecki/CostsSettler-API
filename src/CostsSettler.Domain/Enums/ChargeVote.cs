namespace CostsSettler.Domain.Enums;

/// <summary>
/// Charge vote types.
/// </summary>
public enum ChargeVote : byte
{
    /// <summary>
    /// Empty charge vote type.
    /// </summary>
    None = 0,

    /// <summary>
    /// Accept charge.
    /// </summary>
    Accept = 1,

    /// <summary>
    /// Reject charge.
    /// </summary>
    Reject = 2,
}
