namespace CostsSettler.Auth.Models;
public class KcUser
{
    public Guid Id { get; set; }
    public long CreatedTimestamp { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Enabled { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
