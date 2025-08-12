using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Account.Domain.Mappers;
using LRA.Account.Infrastructure.Persistence.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Test;
using LRA.Shared.Utils.Constants;
using LRA.Shared.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IAccountDbContext _context;

    public AccountRepository(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(AccountEntity model, CancellationToken cancellationToken)
    {    
        _context.Accounts.Add(model);
        await _context.SaveChangesAsync(cancellationToken); 

        return model.Id;
    }

    public async Task<AccountDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts
            .Include(x => x.Roles)
            .Include(x => x.CredentialsDetails)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (model == null)
        {
            throw new NotFoundException(nameof(model), id);
        }

        return model.FromModelToDetailsDto();
    }

    public async Task<AccountDetailsDto> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts
            .Include(x => x.Roles)
            .Include(x => x.CredentialsDetails)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (model == null)
        {
            throw new NotFoundException(nameof(model), email);
        }
        return model.FromModelToDetailsDto();
    }

    public async Task<AccountDetailsDto> GetByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts
            .Include(x => x.Roles)
            .Include(x => x.CredentialsDetails)
            .FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);

        if (model == null)
        {
            throw new NotFoundException(nameof(model), keycloakId);
        }

        return model.FromModelToDetailsDto();
    }

    public async Task<CredentialsDetailsEntity?> GetCredentialsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .Include(x => x.CredentialsDetails)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), email);
        }

        var credentials = entity.CredentialsDetails;
        return credentials;
    }

    public PaginatedList<AccountSummaryDto> FilterAndSearhAccountsAsync(int pageNumber, int pageSize, FilterAndSearchModel model)
    {
        var query = _context.Accounts
        .Include(a => a.Roles)
        .AsQueryable();

        if (model.IsDeleted.HasValue)
            query = query.IgnoreQueryFilters();

        if (!string.IsNullOrWhiteSpace(model.Search))
        {
            query = query.Where(a =>
                (a.FirstName != null && a.FirstName.Contains(model.Search)) ||
                (a.LastName != null && a.LastName.Contains(model.Search)) ||
                a.Email.Contains(model.Search) ||
                (a.PhoneNumber != null && a.PhoneNumber.Contains(model.Search))
            );
        }

        if (model.IsDeleted.HasValue)
            query = query.Where(a => a.IsDeleted == model.IsDeleted.Value);

        if (model.RequiredRoles is { Count: > 0 })
        {
            query = query.Where(a => a.Roles.Any(r => model.RequiredRoles.Contains(r.RoleName)));
        }

        var items = query.Select(x => x.FromModelToSummaryDto());
        var paginatedItems = new PaginatedList<AccountSummaryDto>(items, pageNumber, pageSize);

        return paginatedItems;
    }

    public PaginatedList<AccountSummaryDto> GetPaginatedAccountsAsync(int pageNumber, int pageSize)
    {
        var items = _context.Accounts.Select(x => x.FromModelToSummaryDto());
        var paginatedItems = new PaginatedList<AccountSummaryDto>(items, pageNumber, pageSize);
        return paginatedItems;
    }

    public async Task<List<AccountSummaryDto>> GetAllAccountsAsync(CancellationToken cancellationToken)
    {
        var items = _context.Accounts.Select(x => x.FromModelToSummaryDto()).IgnoreQueryFilters();
        return await items.ToListAsync(cancellationToken);
    }

    public async Task<List<string>> GetRolesByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.Include(a => a.Roles).FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);

        var roles = entity?.Roles.Select(x => x.RoleName).ToList() ?? new List<string>();

        return roles;
    }

    public void SoftDelete(Guid id)
    {
        var entity = _context.Accounts.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOnUtc = DateTime.UtcNow;
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        SoftDelete(id);
        await _context.SaveChangesAsync();
    }

    public void Delete(Guid id)
    {
        var entity = _context.Accounts.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            _context.Accounts.Remove(entity);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        Delete(id);
        await _context.SaveChangesAsync();
    }

    public void Update(UpdateAccountDto dto, Guid keycloakId)
    {
        var entity = _context.Accounts.FirstOrDefault(a => a.KeycloakId == keycloakId);
        if (entity == null)
            throw new NotFoundException(nameof(entity), keycloakId);

        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.PhoneNumber = dto.PhoneNumber;

        entity.UpdatedUtc = DateTime.UtcNow;
    }

    public async Task UpdateAsync(UpdateAccountDto dto, Guid keycloakId, CancellationToken cancellationToken)
    {
        Update(dto, keycloakId);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTwoFaAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }

        entity.IsTwoFaEnabled = !entity.IsTwoFaEnabled;
        entity.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateEmailAsync(string newEmail, Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId);
        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }

        entity.Email = newEmail;
        entity.UpdatedUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.KeycloakId == keycloakId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }

        var roleFromDb = await _context.Roles
            .FirstOrDefaultAsync(r => roleName == r.RoleName);


        if (!entity.Roles.Contains(roleFromDb)) 
        {
            entity.Roles.Add(roleFromDb);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemoveRoleAsync(string roleName, Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.KeycloakId == keycloakId, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), keycloakId);
        }

        var roleFromDb = await _context.Roles
            .FirstOrDefaultAsync(r => roleName == r.RoleName);


        if (entity.Roles.Contains(roleFromDb))
        {
            entity.Roles.Remove(roleFromDb);
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MakeUserVerifiedAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(entity), email);

        entity.IsEmailVerified = true;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsAccountExists(Guid keycloakId, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);
        return !(entity == null);
    }

    public async Task MakeTempPasswordUsedAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts
            .Include(x => x.CredentialsDetails)
            .FirstOrDefaultAsync(a => a.Email == email, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(entity), email);
        }

        if(entity.CredentialsDetails != null)
        {
            entity.CredentialsDetails.IsPasswordUsed = true;
        }
    }

    public async Task HasUserPermission(Guid id, Guid adminId, CancellationToken cancellationToken)
    {
        var currentUser = await _context.Accounts
        .Include(a => a.Roles)
        .FirstOrDefaultAsync(a => a.KeycloakId == adminId, cancellationToken);

        if (currentUser == null)
            throw new NotFoundException(nameof(currentUser), adminId);

        if (currentUser.Id == id)
            throw new PermissionException("Cant modify themself");


        var targetUser = await _context.Accounts
            .Include(a => a.Roles)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (targetUser == null)
            throw new NotFoundException(nameof(targetUser), id);

        bool currentIsSuperAdmin = currentUser.Roles.Any(r => r.RoleName == SystemRoles.SuperAdmin);
        bool targetIsAdminOrSuperAdmin = targetUser.Roles.Any(r =>
            r.RoleName == SystemRoles.Admin || r.RoleName == SystemRoles.SuperAdmin);

        if (!currentIsSuperAdmin && targetIsAdminOrSuperAdmin)
            throw new PermissionException("Admin cannot modify other admins");
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
