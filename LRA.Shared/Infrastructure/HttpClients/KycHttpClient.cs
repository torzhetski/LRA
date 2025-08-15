using LRA.Shared.Abstractions.Dtos.Kyc;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Infrastructure.Settings;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

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

    public async Task ApproveKycAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("approve", new Dictionary<string, string?>
        {
            ["applicationId"] = applicationId.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task RejectKycAsync(Guid applicationId, string rejectMessage, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("reject", new Dictionary<string, string?>
        {
            ["applicationId"] = applicationId.ToString(),
            ["rejectMessage"] = rejectMessage,
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<PaginatedList<KycSummaryDto>> GetAllPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("paginated/all", new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<PaginatedList<KycSummaryDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        return items;
    }

    public async Task<PaginatedList<KycSummaryDto>> GetInProgressPaginatedKyc(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("paginated/in-progress", new Dictionary<string, string?>
        {
            ["pageNumber"] = pageNumber.ToString(),
            ["pageSize"] = pageSize.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<PaginatedList<KycSummaryDto>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        return items;
    }

    public async Task<KycDetailsDto> GetKycDetailsAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("", new Dictionary<string, string?>
        {
            ["applicationId"] = applicationId.ToString(),
        });

        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("InternalApiKey", _apiKey);

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new AlreadyExistsException("Job application");
                case HttpStatusCode.NotFound:
                    throw new NotFoundException("Job application", applicationId);
                default:
                    throw new Exception("Problems with job application");
            }

        var kycDetails = await response.Content.ReadFromJsonAsync<KycDetailsDto>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        return kycDetails;
    }
}
