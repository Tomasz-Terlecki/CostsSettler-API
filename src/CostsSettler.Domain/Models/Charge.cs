using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Models;
public class Charge : ModelBase
{
    public User Creditor { get; set; } = null!;
    public Guid CreditorId { get; set; }
    public User Debtor { get; set; } = null!;
    public Guid DebtorId { get; set; }
    public Circumstance Circumstance { get; set; } = null!;
    public Guid CircumstanceId { get; set; }
    public decimal Amount { get; set; }
    public ChargeStatus ChargeStatus { get; set; }
    public DateTime DateTime { get => Circumstance.DateTime; }
}
