namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface IMobileDataConfiguration
    {
        string Test { get; }
        string ProviderEmail { get; }
        string ProviderPassword { get; }
    }
}