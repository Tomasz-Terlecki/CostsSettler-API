using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;

namespace CostsSettler.Domain.Models;

/// <summary>
/// Domain model representing circumstance.
/// </summary>
public class Circumstance : ModelBase
{
    /// <summary>
    /// Circumstance description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Circumstance amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Charges for the circumstance.
    /// </summary>
    public ICollection<Charge>? Charges { get; set; }
    
    /// <summary>
    /// Date and time of circumstance.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Circumstance status.
    /// </summary>
    public CircumstanceStatus CircumstanceStatus { get; set; }

    /// <summary>
    /// Debtors calculated by charges.
    /// </summary>
    public ICollection<User> Debtors
    { 
        get => Charges?.Select(charge => charge.Debtor).ToList()
            ?? throw new ObjectReferenceException("Failed to get debtors from database");
    }

    /// <summary>
    /// Creditor calculated by charges.
    /// </summary>
    public User Creditor
    { 
        get => Charges?.FirstOrDefault()?.Creditor
            ?? throw new ObjectReferenceException("Failed to get creditor from database");
    }

    /// <summary>
    /// Members calculated by charges.
    /// </summary>
    public ICollection<User> Members
    {
        get => Debtors.Append(Creditor).ToList();
    }

    /// <summary>
    /// Fixes circumstance status by analizing charges statuses.
    /// </summary>
    public void FixCircumstanceStatus()
    {
        if (Charges is null)
            throw new ObjectReferenceException("Failed fixing circumstance status - Charges property is null");
        
        if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.New))
            CircumstanceStatus = CircumstanceStatus.New;
        else if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.Accepted))
            CircumstanceStatus = CircumstanceStatus.Accepted;
        else if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.Settled))
            CircumstanceStatus = CircumstanceStatus.Settled;
        else if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Rejected))
            CircumstanceStatus = CircumstanceStatus.Rejected;
        else if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Settled))
            CircumstanceStatus = CircumstanceStatus.PartiallySettled;
        else if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Accepted))
            CircumstanceStatus = CircumstanceStatus.PartiallyAccepted;
        else CircumstanceStatus = CircumstanceStatus.None;
    }
}
