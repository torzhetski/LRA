using LRA.Account.Application.Interfaces.Repositories;
using LRA.Account.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Account.Infrasturcture.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IAccountDbContext _context;

    public AccountRepository(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(TestModel model)
    {
        _context.TestModels.Add(model);
        await _context.SaveChangesAsync();

        return model.Id;
    }

    public async Task<TestModel> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var model = await _context.TestModels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (model == null) 
        {
            throw new ArgumentNullException(nameof(model));
        }

        return model;
    }

    public void SoftDelete(Guid id)
    {
        var entity = _context.TestModels.FirstOrDefault(x => x.Id == id);
        if (entity != null) 
        {
            entity.IsDeleted = true;
            entity.DeletedOnUtc = DateTime.UtcNow;
        }
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.TestModels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedOnUtc = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public void Delete(Guid id)
    {
        var entity = _context.TestModels.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            _context.TestModels.Remove(entity);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.TestModels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity != null)
        {
            _context.TestModels.Remove(entity);
        }

        await _context.SaveChangesAsync();
    }
    
    public PaginatedList<TestDto> GetPaginatedData(int pageNumber, int pageSize)
    {
        var items = _context.TestModels.Select(x => x.FromModelToDto());
        var paginatedItems = new PaginatedList<TestDto>(items, pageNumber, pageSize);
        return paginatedItems;
    }

    public async Task<List<TestDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = _context.TestModels.Select(x => x.FromModelToDto());
        return await items.ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
