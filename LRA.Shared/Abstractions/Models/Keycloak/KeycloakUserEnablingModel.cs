using System.Text.Json.Serialization;

namespace LRA.Shared.Abstractions.Models.Keycloak;

public class KeycloakUserEnablingModel
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}
