using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Test;

namespace LRA.Subscription.Infrasturcture.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ISubscriptionDbContext _context;

    public SubscriptionRepository(ISubscriptionDbContext context)
    {
        _context = context;
    }

    public Guid Create(TestModel model)
    {
        model.Id = Guid.NewGuid();
        _context.TestModels.Add(model);
        return model.Id;
    }

    public TestModel GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<TestModel> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public void Update(TestModel model)
    {
        throw new NotImplementedException();
    }

    public void Delete(TestModel model)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
