using LRA.Account.Application.Interfaces;
using LRA.Account.Domain.Entities;
using LRA.Account.Presentation.Interfaces;
using LRA.Account.Presentation.Settings;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Utils.Constants;
using LRA.Shared.Utils.Exceptions;
using Microsoft.Extensions.Options;
using System.Security.Principal;

namespace LRA.Account.Presentation.Utils;

public class Seeder
{
    private readonly IAccountService _accountService;
    private readonly IKeycloakService _keycloakService;
    private readonly IOptions<AdminSettings> _settings;

    public Seeder(IKeycloakService keycloakService, IAccountService accountService, IOptions<AdminSettings> settings)
    {
        _keycloakService = keycloakService;
        _accountService = accountService;
        _settings = settings;
    }

    public async Task SeedAdminIfNotExistsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var creationModel = new RegistrationModel()
            {
                Password = _settings.Value.AdminPassword,
                Email = _settings.Value.AdminEmail,
            };

            var keycloakIdStr = await _keycloakService.CreateKeycloakUserAsync(creationModel, cancellationToken);
            var keycloakId = Guid.Parse(keycloakIdStr);

            await _keycloakService.EnableKeycloakUserAsync(keycloakId, cancellationToken);

            await _accountService.CreateAccountAsync(_settings.Value.AdminEmail, keycloakId, cancellationToken);
            await _accountService.AddRoleAsync(SystemRoles.SuperAdmin, keycloakId, cancellationToken);
        }
        catch (AlreadyExistsException ex) 
        {
            return;
        }
    }
}
