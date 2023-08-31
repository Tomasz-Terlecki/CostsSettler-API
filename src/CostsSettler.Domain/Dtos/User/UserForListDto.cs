namespace CostsSettler.Domain.Dtos;

/// <summary>
/// DTO for users list.
/// </summary>
public class UserForListDto
{
    /// <summary>
    /// User id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User first name.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// User last name.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; } = null!;
}
