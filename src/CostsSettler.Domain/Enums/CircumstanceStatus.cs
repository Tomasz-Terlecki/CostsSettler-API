namespace CostsSettler.Domain.Enums;

/// <summary>
/// Status of charge.
/// </summary>
public enum CircumstanceStatus : byte
{
    /// <summary>
    /// Empty circumtance status.
    /// </summary>
    None = 0,

    /// <summary>
    /// Circumstance is new.
    /// </summary>
    New = 1,

    /// <summary>
    /// Circumstance is accepted by some debtors and is not rejected by anyone.
    /// </summary>
    PartiallyAccepted = 2,

    /// <summary>
    /// Circumstance is accepted by all debtors.
    /// </summary>
    Accepted = 3,

    /// <summary>
    /// Circumstance is settled by some debtors.
    /// </summary>
    PartiallySettled = 4,

    /// <summary>
    /// Circumstance is settled by all debtors.
    /// </summary>
    Settled = 5,

    /// <summary>
    /// Circumstance is rejected by at least one debtor.
    /// </summary>
    Rejected = 6,
}
