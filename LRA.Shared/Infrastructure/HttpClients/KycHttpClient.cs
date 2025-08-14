using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace LRA.Shared.Infrastructure.HttpClients;

public class KycHttpClient : IKycHttpClient
{
    private HttpClient _httpClient;
    private readonly string _apiKey;

    public KycHttpClient(HttpClient httpClient, IOptions<KycEndpointsSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
    }

    public async Task ApplyToJobAsync(
        string id,
        IFormFile identityFile,
        IFormFile certificationFile,
        IFormFile selfieFile,
        CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();

        void AddFile(IFormFile file, string name)
        {
            if (file == null) return;

            var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, name, file.FileName);
        }

        AddFile(identityFile, nameof(identityFile));
        AddFile(certificationFile, nameof(certificationFile));
        AddFile(selfieFile, nameof(selfieFile));

        var url = QueryHelpers.AddQueryString("", new Dictionary<string, string?>
        {
            ["id"] = id,
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{fileName}");
        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
