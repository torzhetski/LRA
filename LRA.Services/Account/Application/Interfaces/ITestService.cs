using LRA.Shared.Abstractions.Test;

namespace LRA.Account.Application.Interfaces;

public interface ITestService
{
    TestMessageModel TestResponse(int id);
    Task<TestMessageModel> TestCreateDataResponse(TestModel model, CancellationToken cancellationToken);
    Task<TestDto> TestGetDataResponse(Guid id, CancellationToken cancellationToken);
    Task TestSoftDeleteDataResponse(Guid id, CancellationToken cancellationToken);
}
