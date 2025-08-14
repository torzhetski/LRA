namespace LRA.Account.Infrastructure.Azure.Settings;

public class AzureStorageSettings
{
    public required string ConnectionString { get; set;}
    public required string ContainerName { get; set;}
}
