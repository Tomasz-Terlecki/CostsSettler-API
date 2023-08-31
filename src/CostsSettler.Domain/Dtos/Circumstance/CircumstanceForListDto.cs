using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;

/// <summary>
/// DTO for circumstance list.
/// </summary>
public class CircumstanceForListDto
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
    /// Circumstance amount.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Circumstance date and time.
    /// </summary>
    public DateTime DateTime { get; set; }
}
