using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Domain.Defenitions;
using LRA.Subscription.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using LRA.Shared.Utils.Exceptions;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Utils.Constants;

namespace LRA.Subscription.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IStripeEventConverter _converter;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRabbitMQProducerService _producerService;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(
        IStripeEventConverter converter,
        ISubscriptionRepository subscriptionRepository,
        ILogger<SubscriptionService> logger,
        ICustomerRepository customerRepository,
        IRabbitMQProducerService producerService)
    {
        _converter = converter;
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
        _customerRepository = customerRepository;
        _producerService = producerService;
    }

    public async Task HandleWebhooksAsync(StripeWebhookModel webhookModel, CancellationToken cancellationToken)
    {
        try
        {
            var eventDTO = _converter.ParseAndValidate(webhookModel);
        
            switch (eventDTO.EventType)
            {
                case EventTypes.CustomerSubscriptionCreated:
                    {
                        var subscription = eventDTO.Entity;
                        await _subscriptionRepository.AddSubsriptionAsync(subscription, cancellationToken);

                        var customer = await _customerRepository.GetKeycloakIdOrDefaultByCustomerAysnc(subscription.CustomerId, cancellationToken);
                        if (customer == null)
                            throw new Exception("Adding role failed");
                        var roles = new UpdateRolesModel()
                        {
                            KeycloakId = customer.KeycloakId,
                            RoleNames = new List<string>() { SystemRoles.Client },
                        };
                        await _producerService.SendMessageAsync(roles, cancellationToken);

                        break;
                    }
                case EventTypes.CustomerSubscriptionUpdated:
                    await _subscriptionRepository.UpdateSubsriptionAsync(eventDTO.Entity, cancellationToken);
                    break;
                case EventTypes.CustomerSubscriptionDeleted:
                    {
                        var subscription = eventDTO.Entity;
                        await _subscriptionRepository.UpdateSubsriptionAsync(subscription, cancellationToken);

                        var customer = await _customerRepository.GetKeycloakIdOrDefaultByCustomerAysnc(subscription.CustomerId, cancellationToken);
                        if (customer == null)
                            throw new Exception("Removinh role failed");

                        var roles = new UpdateRolesModel()
                        {
                            KeycloakId = customer.KeycloakId,
                            RoleNames = new List<string>() {},
                        };
                        await _producerService.SendMessageAsync(roles, cancellationToken);

                        break;
                    }
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
