using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;
public class CircumstanceForListDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public CircumstanceStatus CircumstanceStatus { get; set; }
    public DateTime DateTime { get; set; }
}
