namespace Shared.Abstractions.Interfaces;

public interface IHttpClientService
{
    Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request, CancellationToken cancellationToken = default);
}
