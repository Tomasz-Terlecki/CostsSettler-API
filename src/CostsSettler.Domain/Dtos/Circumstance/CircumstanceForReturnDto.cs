using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Dtos;
public class CircumstanceForReturnDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public ICollection<Charge>? Charges { get; set; }
}
