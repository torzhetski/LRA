using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class KeycloakHttpClient : IKeycloakHttpClient
{
    private HttpClient _httpClient;

    public KeycloakHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetServiceSettingsAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $".well-known/openid-configuration");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content;
    }

    public Task<string> CreateUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
