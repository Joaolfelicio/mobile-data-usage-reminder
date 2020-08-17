namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface IApplicationConfiguration
    {
        string Test { get; }
        string ProviderEmail { get; }
        string ProviderPassword { get; }
    }
}