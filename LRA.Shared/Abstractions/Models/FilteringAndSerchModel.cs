namespace LRA.Shared.Abstractions.Models;

public class FilterAndSearchModel
{
    public string? Search {  get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsTwoFaEnabled { get; set; }
    public bool? IsBlocked { get; set; }
    public List<string> RequiredRoles { get; set; } = new List<string>();
}
