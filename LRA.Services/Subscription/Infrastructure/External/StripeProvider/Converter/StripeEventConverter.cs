using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Application.Dto;
using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Mappers;
using LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;
using Microsoft.Extensions.Options;
using Stripe;


namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Converter;

public class StripeEventConverter : IStripeEventConverter
{
    private readonly IOptions<StripeSettings> _options;

    public StripeEventConverter(IOptions<StripeSettings> options)
    {
        _options = options;
    }

    public SubscriptionEventDto ParseAndValidate(StripeWebhookModel webhookModel)
    {
        var stripeEvent = EventUtility.ConstructEvent(webhookModel.Body, webhookModel.StripeHeaders, _options.Value.WebHookSecret);

        var subscription = (stripeEvent.Data.Object as Stripe.Subscription)!.FromStripeModelToEntity();
        var type = stripeEvent.Type;

        return new SubscriptionEventDto
        {
            Entity = subscription,
            EventType = type
        };
    }
}
