using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Domain.Defenitions;
using LRA.Subscription.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using LRA.Shared.Utils.Exceptions;

namespace LRA.Subscription.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IStripeEventConverter _converter;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(IStripeEventConverter converter, ISubscriptionRepository subscriptionRepository, ILogger<SubscriptionService> logger)
    {
        _converter = converter;
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task HandleWebhooksAsync(StripeWebhookModel webhookModel, CancellationToken token)
    {
        try
        {
            var eventDTO = _converter.ParseAndValidate(webhookModel);
        
            switch (eventDTO.EventType)
            {
                case EventTypes.CustomerSubscriptionCreated:
                    await _subscriptionRepository.AddSubsriptionAsync(eventDTO.Entity, token);
                    break;
                case EventTypes.CustomerSubscriptionUpdated:
                    await _subscriptionRepository.UpdateSubsriptionAsync(eventDTO.Entity, token);
                    break;
                case EventTypes.CustomerSubscriptionDeleted:
                    await _subscriptionRepository.UpdateSubsriptionAsync(eventDTO.Entity, token);
                    break;
                default:
                    throw new UnknownEventTypeException(eventDTO.EventType);

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<bool> HasCustomerActiveSubscription(string customerId, CancellationToken cancellationToken)
    {
        var response = await _subscriptionRepository.HasCustomerActiveSubscription(customerId, cancellationToken);
        return response;
    }
}
