using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Exceptions;

namespace CostsSettler.Domain.Models;
public class Circumstance : ModelBase
{
    public string Description { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public ICollection<Charge>? Charges { get; set; }
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
}
