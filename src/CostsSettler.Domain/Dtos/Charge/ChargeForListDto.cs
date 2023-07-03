using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;
public class ChargeForListDto
{
    public Guid Id { get; set; }
    public Guid CreditorId { get; set; }
    public Guid DebtorId { get; set; }
    public Guid CircumstanceId { get; set; }
    public string CircumstanceDescription { get; set; } = null!;
    public decimal Amount { get; set; }
    public ChargeStatus ChargeStatus { get; set; }
}
