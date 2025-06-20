using LRA.Shared.Abstructions.Test;
using Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestAccountService
{
    Task<TestMessageModel> TestAccountServiceAsync(int id, CancellationToken cancellationToken);
    Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestModel model, CancellationToken cancellationToken);
}
