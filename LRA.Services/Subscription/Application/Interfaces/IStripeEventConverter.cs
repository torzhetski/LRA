using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Application.Dto;

namespace LRA.Subscription.Application.Interfaces;

public interface IStripeEventConverter
{
    SubscriptionEventDto ParseAndValidate(StripeWebhookModel webhookModel);
}
