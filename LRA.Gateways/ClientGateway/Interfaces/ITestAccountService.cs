using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestAccountService
{
    Task<TestDto> TestAccountServiceAsync(Guid id, CancellationToken cancellationToken);
    Task<TestMessageModel> TestAccountDatabaseServiceAsync(TestDto model, CancellationToken cancellationToken);
    Task TestAccountSoftDelete(Guid id, CancellationToken cancellationToken);
    Task TestBrokerAsync(string message, CancellationToken cancellationToken);
}
