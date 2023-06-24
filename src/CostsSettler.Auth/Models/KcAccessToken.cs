namespace CostsSettler.Auth.Models;
public class KcAccessToken
{
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public int RefreshExpiresIn { get; set; }
    public string TokenType { get; set; } = null!;
    public int NotBeforePolicy { get; set; }
    public string Scope { get; set; } = null!;
}
