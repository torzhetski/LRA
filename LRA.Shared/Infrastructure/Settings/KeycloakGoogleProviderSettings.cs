namespace LRA.Shared.Infrastructure.Settings;

public class KeycloakGoogleProviderSettings
{
    public required string ClientId { get; set; }
    public required string RedirectUri { get; set; }
    public required string BaseServerUrl { get; set; }
    public required string Realm { get; set; }
}
