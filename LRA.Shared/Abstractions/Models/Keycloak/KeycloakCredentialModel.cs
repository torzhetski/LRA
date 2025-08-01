using System.Text.Json.Serialization;

namespace LRA.Shared.Abstractions.Models.Keycloak;

public class KeycloakCredentialModel
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }
    [JsonPropertyName("value")]
    public required string Value { get; set; }
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; }
}
