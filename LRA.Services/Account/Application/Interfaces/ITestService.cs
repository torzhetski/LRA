using LRA.Shared.Abstructions.Test;
using Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces;

public interface ITestService
{
    TestMessageModel TestResponse(int id);
    Task<TestMessageModel> TestDatabaseResponse(TestModel model, CancellationToken cancellationToken);
}
