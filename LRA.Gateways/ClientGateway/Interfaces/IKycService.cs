using LRA.Shared.Abstractions.Models;

namespace LRA.Gateways.Client.Interfaces;

public interface IKycService
{
    Task ApplyToJobAsync(string id, IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken);
}
