namespace CostsSettler.Auth.Config;
public class KeycloakClientConfig
{
    public string Realm { get; set; } = null!;
    public string AuthServerUrl { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string ClientId { get; set; } = null!;
}
