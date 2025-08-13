using LRA.Account.Domain.Entities;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Account.Domain.Mappers;

public static class AccountMapper
{
    public static AccountDetailsDto FromModelToDetailsDto (this AccountEntity model)
    {
        return new AccountDetailsDto()
        {
            Id = model.Id,
            KeycloakId = model.KeycloakId,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            CreatedUtc = model.CreatedUtc,
            UpdatedUtc = model.UpdatedUtc,
            IsTwoFaEnabled = model.IsTwoFaEnabled,
            IsDeleted = model.IsDeleted,
            IsEmailVerified = model.IsEmailVerified,
            UsedTempPassword = model.CredentialsDetails == null ? null : model.CredentialsDetails.IsPasswordUsed,
            IsBlocked = model.IsBlocked,
            BlockedUntil = model.BlockedUntil,
            Roles = model.Roles.Select(x => x.RoleName).ToList(),
        };
    }

    public static AccountSummaryDto FromModelToSummaryDto(this AccountEntity model)
    {
        return new AccountSummaryDto()
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
        };
    }
}
