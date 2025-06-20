using Shared.Abstractions.Test;

namespace LRA.Shared.Abstractions.Interfaces.HttpClients;

public interface IAuthHttpClient
{
    Task<TestMessageModel> GetAsync(int id, CancellationToken cancellationToken);
}
