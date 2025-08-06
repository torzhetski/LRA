namespace LRA.Shared.Infrastructure.Settings;

public class KeycloakServiceSettings
{
    public required string AdminClientId { get; set; }
    public required string PublicClientId { get; set; }
    public required string Username {  get; set; }
    public required string Password { get; set; }
    public required string AdminBaseUrl { get; set; }
    public required string AccessTokenUrl { get; set; }
    public required string KeycloakBaseUrl { get; set; }
    public required string GoogleLoginRedirectUrl { get; set; }
}
