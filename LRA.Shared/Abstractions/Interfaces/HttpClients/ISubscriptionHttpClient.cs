using LRA.Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface ISubscriptionHttpClient
{
    Task<TestMessageModel> GetAsync(int id, CancellationToken cancellationToken);
}
