using LRA.Auth.Application.Interfaces.Repositories;
using LRA.Auth.Infrasturcture.Persistence.Interfaces;
using LRA.Shared.Abstractions.Test;

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
        model.Id = Guid.NewGuid();
        _context.TestModels.Add(model);
        return model.Id;
    }

    public TestModel GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<TestModel> GetAllAsync()
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
