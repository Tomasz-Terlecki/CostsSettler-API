using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Models;
public class MemberCharge : ModelBase
{
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Circumstance Circumstance { get; set; } = null!;
    public Guid CircumstanceId { get; set; }
    public CircumstanceRole CircumstanceRole { get; set; }
    public decimal Amount { get; set; }
    public ChargeStatus ChargeStatus { get; set; }
}
