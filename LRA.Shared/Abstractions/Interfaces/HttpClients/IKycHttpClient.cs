using LRA.Shared.Abstractions.Models;
using Microsoft.AspNetCore.Http;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IKycHttpClient
{
    Task<RedirectUrlModel> UploadPhotoAsync(IFormFile file, CancellationToken cancellationToken);
    Task DeletePhotoAsync(string fileName, CancellationToken cancellationToken);
}
