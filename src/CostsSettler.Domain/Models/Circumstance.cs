using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;

namespace CostsSettler.Domain.Models;
public class Circumstance : ModelBase
{
    public string Description { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public ICollection<Charge>? Charges { get; set; }
    public DateTime DateTime { get; set; }

    public CircumstanceStatus CircumstanceStatus 
    { 
        get
        {
            if (Charges is null)
                throw new ObjectReferenceException("Failed to get circumstance status from database");
            
            if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.New))
                return CircumstanceStatus.New;

            if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.Accepted))
                return CircumstanceStatus.Accepted;

            if (Charges.All(charge => charge.ChargeStatus == ChargeStatus.Settled))
                return CircumstanceStatus.Settled;

            if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Rejected))
                return CircumstanceStatus.Rejected;

            if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Settled))
                return CircumstanceStatus.PartiallySettled;

            if (Charges.Any(charge => charge.ChargeStatus == ChargeStatus.Accepted))
                return CircumstanceStatus.PartiallyAccepted;

            return CircumstanceStatus.None;
        }
    }
    public ICollection<User> Debtors 
    { 
        get => Charges?.Select(charge => charge.Debtor).ToList()
            ?? throw new ObjectReferenceException("Failed to get debtors from database");
    }
    public User? Creditor
    { 
        get => Charges?.FirstOrDefault()?.Creditor
            ?? throw new ObjectReferenceException("Failed to get creditor from database");
    }
    public ICollection<User> Members
    {
        get => (ICollection<User>)Debtors.Append(Creditor);
    }
}
