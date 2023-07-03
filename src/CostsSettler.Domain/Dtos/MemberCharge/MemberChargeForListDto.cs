using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;
public class MemberChargeForListDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CircumstanceId { get; set; }
    public string CircumstanceDescription { get; set; } = null!;
    public decimal Amount { get; set; }
    public ChargeStatus ChargeStatus { get; set; }
}
