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

    public async Task<string> GetCustomerId(Guid keycloakId, string email, CancellationToken cancellationToken)
    {
        var entity = await _customerRepository.GetCustomerOrDefaultByKeycloakIdAysnc(keycloakId, cancellationToken);
        if (entity == null) 
        {
            var customerId = await _stripeService.CreateStripeCustomerAsync(email, cancellationToken);

            entity = new CustomerEntity()
            {
                KeycloakId = keycloakId,
                StripeCustomerId = customerId,
            };
            await _customerRepository.CreateCustomerAsync(entity, cancellationToken);

            return customerId;
        }
        else
        {
            return entity.StripeCustomerId;
        }
    }
}
