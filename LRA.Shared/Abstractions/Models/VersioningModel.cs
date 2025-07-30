namespace LRA.Shared.Abstractions.Models;

public class VersioningModel
{
    public required string Version { get; set; }
    public bool IsSupported { get; set; }
}
