using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKycHttpClient
{
    Task ApplyToJobAsync(string id, IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken);
}
