using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;
public class CircumstanceForListDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public Guid CreditorId { get; set; }
    public UserForListDto Creditor { get; set; } = null!;
    public Guid DebtorId { get; set; }
    public UserForListDto Debtor { get; set; } = null!;
    public DateTime DateTime { get; set; }
}
