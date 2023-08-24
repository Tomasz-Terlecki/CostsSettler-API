using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;
public class RandomCircumstanceFactory : RandomFactoryBase<Circumstance, CircumstanceAttributes>
{
    protected override Circumstance CreateModel(Guid id, CircumstanceAttributes? attributes = null)
    {
        return new Circumstance
        { 
            Id = id,
            CircumstanceStatus = attributes?.CircumstanceStatus ?? CircumstanceStatus.New,
            DateTime = attributes?.DateTime ?? DateTime.UtcNow,
            Description = attributes?.Description ?? "description" + id,
            TotalAmount = attributes?.TotalAmount ?? 100,
            Charges = attributes?.Charges ?? new List<Charge>(),
        };
    }
}

public class CircumstanceAttributes
{
    public string? Description { get; set; }
    public decimal? TotalAmount { get; set; }
    public ICollection<Charge>? Charges { get; set; }
    public DateTime? DateTime { get; set; }
    public CircumstanceStatus? CircumstanceStatus { get; set; }
}
