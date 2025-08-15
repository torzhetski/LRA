using LRA.Shared.Abstractions.Models;

namespace LRA.Account.Application.Interfaces;

public interface IAzureService
{
    Task<RedirectUrlModel> UploadPhotoAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken = default);
    Task<string> GetFileBytesAsync(string blobUrl, CancellationToken cancellationToken);
}
