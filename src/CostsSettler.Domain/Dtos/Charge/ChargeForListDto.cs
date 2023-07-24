using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;
public class ChargeForListDto
{
    public Guid Id { get; set; }
    public Guid CreditorId { get; set; }
    public UserForListDto Creditor { get; set; } = null!;
    public Guid DebtorId { get; set; }
    public UserForListDto Debtor { get; set; } = null!;
    public Guid CircumstanceId { get; set; }
    public string CircumstanceDescription { get; set; } = null!;
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public decimal Amount { get; set; }
    public ChargeStatus ChargeStatus { get; set; }
    public DateTime DateTime { get; set; }
}
