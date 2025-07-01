using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<TestDto> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TestMessageModel> PostAsync(TestDto model, CancellationToken cancellationToken);
    Task PostEmailMessageAsync(EmailMessageModel messageModel, CancellationToken cancellationToken);
}
