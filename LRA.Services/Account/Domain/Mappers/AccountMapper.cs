using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Domain.Mappers;

public static class AccountMapper
{
    public static AccountDto FromModelToDto (this AccountEntity model)
    {
        return new AccountDto()
        {
            Id = model.Id,
            KeycloakId = model.KeycloakId,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            CreatedUtc = model.CreatedUtc,
            UpdatedUtc = model.UpdatedUtc,
            Roles = model.Roles,
            IsTwoFaEnabled = model.IsTwoFaEnabled,
        };
    }
}
