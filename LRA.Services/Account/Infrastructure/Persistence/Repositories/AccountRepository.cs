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

    public Guid Create(TestModel model)
    {
        if (model.Id == default)
        {
            model.Id = Guid.NewGuid();
        }

        _context.TestModels.Add(model);
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

    public PaginatedList<TestDto> GetPaginatedData(int pageNumber, int pageSize)
    {
        var items = GetAll();
        var paginatedItems = new PaginatedList<TestDto>(items, pageNumber, pageSize);
        return paginatedItems;
    }

    public async Task<List<TestDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var items = GetAll();
        return await items.ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IQueryable<TestDto> GetAll()
    {
        return _context.TestModels.Select(x => x.FromModelToDto());
    }
}
