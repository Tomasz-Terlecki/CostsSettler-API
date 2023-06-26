namespace CostsSettler.Domain.Models;
public class User : ModelBase
{
    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<MemberCharge>? Charges { get; set; }
}
