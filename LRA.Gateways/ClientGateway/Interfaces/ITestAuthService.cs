using LRA.Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestAuthService
{
    Task<TestMessageModel> TestAuthServiceAsync(int id, CancellationToken cancellationToken);
}
