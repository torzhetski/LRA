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

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
