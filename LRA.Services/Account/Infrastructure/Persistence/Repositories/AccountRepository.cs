using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Domain.Entities;
using LRA.Account.Infrastructure.Persistence.Interfaces;
using LRA.Shared.Abstractions.Dtos.Account;
using LRA.Shared.Abstractions.Test;
using Microsoft.EntityFrameworkCore;
using LRA.Account.Domain.Mappers;
using LRA.Shared.Utils.Exceptions;

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

    public async Task<AccountDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (model == null)
        {
            throw new NotFoundException(nameof(model), id);
        }

        return model.FromModelToDto();
    }

    public async Task<AccountDto> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (model == null)
        {
            throw new NotFoundException(nameof(model), email);
        }
        return model.FromModelToDto();
    }

    public async Task<AccountDto> GetByKeycloakIdAsync(Guid keycloakId, CancellationToken cancellationToken)
    {
        var model = await _context.Accounts.FirstOrDefaultAsync(x => x.KeycloakId == keycloakId, cancellationToken);

        if (model == null)
        {
            throw new NotFoundException(nameof(model), keycloakId);
        }

        return model.FromModelToDto();
    }

    public PaginatedList<AccountDto> GetPaginatedData(int pageNumber, int pageSize)
    {
        var items = _context.Accounts.Select(x => x.FromModelToDto());
        var paginatedItems = new PaginatedList<AccountDto>(items, pageNumber, pageSize);
        return paginatedItems;
    }

    public async Task<List<AccountDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = _context.Accounts.Select(x => x.FromModelToDto());
        return await items.ToListAsync(cancellationToken);
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

    public async Task MakeUserVerifiedAsync(string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
        if (entity == null)
            throw new NotFoundException(nameof(entity), email);

        entity.IsEmailVerified = true;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
