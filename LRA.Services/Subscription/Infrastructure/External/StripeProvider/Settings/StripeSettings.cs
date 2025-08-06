namespace LRA.Subscription.Infrasturcture.External.StripeProvider.Settings;

public class StripeSettings
{
    public required string SecretKey { get; set; }
    public required string WebHookSecret { get; set; }
    public required string SuccessUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required string ReturnUrl { get; set; }
}
