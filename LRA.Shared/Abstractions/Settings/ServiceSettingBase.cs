namespace LRA.Shared.Abstractions.Settings;

public abstract class ServiceSettingBase
{
    public required string ApiKey { get; set; }
    public required string Url { get; set; }
}
