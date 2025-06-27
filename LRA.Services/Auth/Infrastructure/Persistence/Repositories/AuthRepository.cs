using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Test;
using Microsoft.EntityFrameworkCore;

namespace LRA.Auth.Infrasturcture.Persistence.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IAuthDbContext _context;

    public AuthRepository(IAuthDbContext context)
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

    public Task<List<TestModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Update(TestModel model)
    {
        throw new NotImplementedException();
    }

    public void Delete(TestModel model)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
