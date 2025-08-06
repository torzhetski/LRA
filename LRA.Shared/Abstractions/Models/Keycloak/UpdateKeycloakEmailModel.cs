using System.Text.Json.Serialization;

namespace LRA.Shared.Abstractions.Models.Keycloak;

public class UpdateKeycloakEmailModel
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
}
