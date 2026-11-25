using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LRA.Account.Application.Interfaces;
using LRA.Account.Infrastructure.Azure.Settings;
using LRA.Shared.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LRA.Account.Infrastructure.Azure.Services;

public class AzureService : IAzureService
{
    private readonly BlobContainerClient _containerClient;

    public AzureService(IOptions<AzureStorageSettings> azureSettings)
    {
        var blobServiceClient = new BlobServiceClient(azureSettings.Value.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(azureSettings.Value.ContainerName);

        _containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<RedirectUrlModel> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);
        var url = blobClient.Uri.ToString().Replace("azurite", "localhost");

        var urlModel = new RedirectUrlModel()
        {
            RedirectUrl = url,
        };
        return urlModel; 
    }

    public async Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    public async Task<string> GetFileBytesAsync(string blobUrl, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(blobUrl);
        var fileExtension = Path.GetExtension(blobUrl);

        var blobClient = _containerClient.GetBlobClient(fileName);

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream, cancellationToken);
        var fileContent = Convert.ToBase64String(stream.ToArray());
        return fileContent;
    }
}
