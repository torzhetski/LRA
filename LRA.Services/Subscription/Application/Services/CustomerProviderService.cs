using LRA.Shared.Utils.Exceptions;
using LRA.Subscription.Application.Interfaces;
using LRA.Subscription.Application.Interfaces.Repositories;
using LRA.Subscription.Domain.Entities;

namespace LRA.Subscription.Application.Services;

public class CustomerProviderService : ICustomerProviderService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IStripeService _stripeService;

    public CustomerProviderService(ICustomerRepository customerRepository, IStripeService stripeService)
    {
        _customerRepository = customerRepository;
        _stripeService = stripeService;
    }

    public async Task<string> GetCustomerId(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _customerRepository.GetCustomerOrDefaultByKeycloakIdAysnc(keycloakId, cancellationToken);
        if (entity == null) 
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }
        else
        {
            return entity.StripeCustomerId;
        }
    }

    public async Task CreateStripeCustomer(Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var customerId = await _stripeService.CreateStripeCustomerAsync(email, cancellationToken);

        var entity = new CustomerEntity()
        {
            KeycloakId = keycloakId,
            StripeCustomerId = customerId,
            Email = email,
        };
        await _customerRepository.CreateCustomerAsync(entity, cancellationToken);
    }

    public async Task DeleteStripeCustomer(Guid keycloakId, CancellationToken cancellationToken)
    {
        var stripeId = await GetCustomerId(keycloakId, cancellationToken);

        await _customerRepository.DeleteCustomerAsync(keycloakId, cancellationToken);

        await _stripeService.DeleteStripeCustomerAsync(stripeId, cancellationToken);
    }
}
