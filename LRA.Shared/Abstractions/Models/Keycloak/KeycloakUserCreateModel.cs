using System.Text.Json.Serialization;

namespace LRA.Shared.Abstractions.Models.Keycloak;

public class KeycloakUserCreateModel
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    [JsonPropertyName("credentials")]
    public List<KeycloakCredentialModel> Credentials { get; set; } = new();
}
