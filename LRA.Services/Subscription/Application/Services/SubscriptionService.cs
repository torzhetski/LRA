using LRA.Subscription.Application.Interfaces;
using LRA.Shared.Abstractions.Models;
using LRA.Subscription.Domain.Defenitions;
using LRA.Subscription.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using LRA.Shared.Utils.Exceptions;
using LRA.Shared.Abstractions.Interfaces.RabbitMq;
using LRA.Shared.Utils.Constants;
using LRA.Shared.Utils.Enums;
using Microsoft.Extensions.Options;
using LRA.Subscription.Application.Settings;

namespace LRA.Subscription.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IStripeEventConverter _converter;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRabbitMQProducerService _producerService;
    private readonly ILogger<SubscriptionService> _logger;
    private readonly string _queueName;
    public readonly string _queueRoutingKey;

    public SubscriptionService(
        IStripeEventConverter converter,
        ISubscriptionRepository subscriptionRepository,
        ILogger<SubscriptionService> logger,
        ICustomerRepository customerRepository,
        IRabbitMQProducerService producerService,
        IOptions<SubscriptionRabbitMqProducerSettings> options)
    {
        _converter = converter;
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
        _customerRepository = customerRepository;
        _producerService = producerService;
        _queueName = options.Value.RoleUpdateQueue;
        _queueRoutingKey = options.Value.RoleUpdateQueueRoutingKey;
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

                        var role = new UpdateRoleModel()
                        {
                            KeycloakId = customer.KeycloakId,
                            RoleName = SystemRoles.Client,
                            UpdateType = RoleUpdateType.Add
                        };
                        await _producerService.SendMessageAsync(role, _queueName, _queueRoutingKey, cancellationToken);

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
                            throw new Exception("Removing role failed");

                        var role = new UpdateRoleModel()
                        {
                            KeycloakId = customer.KeycloakId,
                            RoleName = SystemRoles.Client,
                            UpdateType = RoleUpdateType.Remove
                        };
                        await _producerService.SendMessageAsync(role, _queueName, _queueRoutingKey, cancellationToken);

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
