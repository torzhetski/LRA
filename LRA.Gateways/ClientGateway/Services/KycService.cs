using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Interfaces.HttpClients;
using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.Client.Services;

public class KycService : IKycService
{
    private readonly IKycHttpClient _httpClient;

    public KycService(IKycHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RedirectUrlModel> UploadPhotoAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var response = await _httpClient.UploadPhotoAsync(file, cancellationToken);
        return response;
    }

    public async Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken)
    {
        await _httpClient.DeletePhotoAsync(fileName, cancellationToken);
    }
}
