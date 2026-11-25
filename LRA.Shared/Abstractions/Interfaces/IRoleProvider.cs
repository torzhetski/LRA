namespace LRA.Shared.Abstractions.Interfaces;

public interface IRoleProvider
{
    Task<List<string>> GetRolesById(string userId, CancellationToken cancellationToken);
}
