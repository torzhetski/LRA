using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Infrasturcture.Persistence.Interfaces;
using LRA.Subscription.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LRA.Shared.Utils.Exceptions;

namespace LRA.Subscription.Infrasturcture.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ISubscriptionDbContext _context;

    public SubscriptionRepository(ISubscriptionDbContext context)
    {
        _context = context;
    }

    public async Task AddSubsriptionAsync(SubscriptionEntity entity, CancellationToken token)
    {
        _context.Subscriptions.Add(entity);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateSubsriptionAsync(SubscriptionEntity entity, CancellationToken token)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.StripeSubscriptionId == entity.StripeSubscriptionId, cancellationToken: token);

        if (subscription == null)
        {
            throw new NotFoundException(nameof(entity), entity.Id);
        }

        subscription.Status = entity.Status;
        subscription.StartDate = entity.StartDate; 
        subscription.EndDate = entity.EndDate;
        subscription.CancelAtThePeriodEnd = entity.CancelAtThePeriodEnd;
        subscription.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync(token);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
