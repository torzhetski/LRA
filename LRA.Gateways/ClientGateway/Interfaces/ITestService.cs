using Shared.Abstractions.Test;

namespace LRA.Gateways.Client.Interfaces;

public interface ITestService
{
    Task<TestMessageModel> TestAuthServiceAsync(int id);
    Task<TestMessageModel> TestAccountServiceAsync(int id);
    Task<TestMessageModel> TestSubscriptionServiceAsync(int id);
}
