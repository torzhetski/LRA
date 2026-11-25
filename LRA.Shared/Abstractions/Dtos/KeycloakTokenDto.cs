using System.Text.Json.Serialization;

namespace LRA.Shared.Abstractions.Dtos;

public class KeycloakTokenDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; }
}
