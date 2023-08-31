using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Tests.Helpers;

/// <summary>
/// Factory for random circumstance.
/// </summary>
public class RandomCircumstanceFactory : RandomFactoryBase<Circumstance, CircumstanceAttributes>
{
    /// <summary>
    /// Creates random circumstance for given attributes.
    /// </summary>
    /// <param name="id">Circumstance id.</param>
    /// <param name="attributes">Circumstance attributes to apply.</param>
    /// <returns>New circumstance object.</returns>
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

/// <summary>
/// Circumstance attributes for RandomCircumstanceFactory.
/// </summary>
public class CircumstanceAttributes
{
    /// <summary>
    /// Circumstance description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Circumstance amount.
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// List of circumstance charges.
    /// </summary>
    public ICollection<Charge>? Charges { get; set; }
    
    /// <summary>
    /// Circumstance date and time.
    /// </summary>
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Status of circumstance.
    /// </summary>
    public CircumstanceStatus? CircumstanceStatus { get; set; }
}
