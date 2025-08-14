using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.Client.Interfaces;

public interface IKycService
{
    Task<RedirectUrlModel> UploadPhotoAsync(IFormFile file, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken);
}
