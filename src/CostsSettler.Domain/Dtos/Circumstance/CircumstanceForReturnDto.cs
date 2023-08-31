using CostsSettler.Domain.Enums;
using CostsSettler.Domain.Models;

namespace CostsSettler.Domain.Dtos;

/// <summary>
/// DTO for circumstance return.
/// </summary>
public class CircumstanceForReturnDto
{
    /// <summary>
    /// Circumstance id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Circumstance description.
    /// </summary>
    public string Description { get; set; } = null!;
    
    /// <summary>
    /// Circumstance status.
    /// </summary>
    public CircumstanceStatus CircumstanceStatus { get; set; }
    
    /// <summary>
    /// Circumstance charges.
    /// </summary>
    public ICollection<Charge>? Charges { get; set; }
}
