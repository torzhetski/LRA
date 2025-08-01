namespace LRA.Account.Domain.Entities;

public class RoleEntity
{
    public Guid Id { get; set; }
    public required string RoleName { get; set; }

    public ICollection<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();
}
