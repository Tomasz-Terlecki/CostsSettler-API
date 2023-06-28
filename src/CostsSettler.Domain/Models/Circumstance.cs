using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Models;
public class Circumstance : ModelBase
{
    public string Description { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public ICollection<MemberCharge>? Members { get; set; }
    public User? Creditor 
    { 
        get => Members?
                .FirstOrDefault(member => member.CircumstanceRole == CircumstanceRole.Creditor)?
                .User 
            ?? throw new Exception("Failed to get creditor from database");
    }
}
