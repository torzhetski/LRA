using LRA.Account.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LRA.Account.Application.Interfaces;

public interface IKycService
{
    Task CreateIfPossibleAsync(IFormFile identityFile, IFormFile certificationFile, IFormFile selfieFile, CancellationToken cancellationToken);
    Task ApproveKycAsync(Guid userId, CancellationToken cancellationToken);
    Task RejectKycAsync(Guid userId, string rejectMessage, CancellationToken cancellationToken);
}
