using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;

namespace CostsSettler.Domain.Models;
public class Circumstance : ModelBase
{
    public string Description { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ICollection<Charge>? Charges { get; set; }
    public DateTime DateTime { get; set; }
    public CircumstanceStatus CircumstanceStatus { get; set; }

    public ICollection<User> Debtors
    { 
        get => Charges?.Select(charge => charge.Debtor).ToList()
            ?? throw new ObjectReferenceException("Failed to get debtors from database");
    }
    public User Creditor
    { 
        get => Charges?.FirstOrDefault()?.Creditor
            ?? throw new ObjectReferenceException("Failed to get creditor from database");
    }
    public ICollection<User> Members
    {
        get => Debtors.Append(Creditor).ToList();
    }

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
