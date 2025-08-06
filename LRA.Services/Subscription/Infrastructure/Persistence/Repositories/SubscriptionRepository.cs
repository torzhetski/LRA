using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Subscription.Infrasturcture.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ISubscriptionDbContext _context;

    public SubscriptionRepository(ISubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
