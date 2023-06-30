using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos.Circumstance;
public class CircumstanceForListDto
{
    public string Description { get; set; } = null!;
    public CircumstanceStatus CircumstanceStatus { get; set; }
}
