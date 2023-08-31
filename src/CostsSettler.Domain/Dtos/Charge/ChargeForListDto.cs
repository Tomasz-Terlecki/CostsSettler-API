using CostsSettler.Domain.Enums;

namespace CostsSettler.Domain.Dtos;

/// <summary>
/// DTO for charge list.
/// </summary>
public class ChargeForListDto
{
    /// <summary>
    /// Charge id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creditor id.
    /// </summary>
    public Guid CreditorId { get; set; }
    
    /// <summary>
    /// Creditor object.
    /// </summary>
    public UserForListDto Creditor { get; set; } = null!;
    
    /// <summary>
    /// Debtor id.
    /// </summary>
    public Guid DebtorId { get; set; }
    
    /// <summary>
    /// Debtor object.
    /// </summary>
    public UserForListDto Debtor { get; set; } = null!;
    
    /// <summary>
    /// Circumstance id.
    /// </summary>
    public Guid CircumstanceId { get; set; }
    
    /// <summary>
    /// Description of circumstance the charge depends to.
    /// </summary>
    public string CircumstanceDescription { get; set; } = null!;
    
    /// <summary>
    /// Status of circumstance the charge depends to.
    /// </summary>
    public CircumstanceStatus CircumstanceStatus { get; set; }
    
    /// <summary>
    /// Charge amount.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Charge status.
    /// </summary>
    public ChargeStatus ChargeStatus { get; set; }
    
    /// <summary>
    /// Charge date and time.
    /// </summary>
    public DateTime DateTime { get; set; }
}
