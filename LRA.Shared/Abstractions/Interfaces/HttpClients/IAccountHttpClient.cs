using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAccountHttpClient
{
    Task<TestMessageModel> GetAsync(int id, CancellationToken cancellationToken);
    Task<TestMessageModel> PostAsync(TestModel model, CancellationToken cancellationToken);
}
