using Shared.Abstractions.Interfaces;
using System.Text.Json;

namespace Shared.Infrastructure;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _client;

    public HttpClientService(HttpClient client)
    {
        _client = client;
    }

    public async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var response = await _client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}