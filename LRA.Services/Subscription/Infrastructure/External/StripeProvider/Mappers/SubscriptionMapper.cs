using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Mappers;

public static class SubscriptionMapper
{
    public static SubscriptionEntity FromStripeModelToEntity(this Stripe.Subscription stripeSubscription)
    {
        return new SubscriptionEntity
        {
            StripeSubscriptionId = stripeSubscription.Id,
            CustomerId = stripeSubscription.CustomerId,
            PriceId = stripeSubscription.Items.Data.FirstOrDefault()?.Price.Id,
            Status = stripeSubscription.Status,
            StartDate = stripeSubscription.StartDate,
            EndDate = stripeSubscription.EndedAt,
            CancelAtThePeriodEnd = stripeSubscription.CancelAtPeriodEnd,
        };
    }
}
