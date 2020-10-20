namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface IApplicationConfiguration
    {
        string ProviderEmail { get; }
        string ProviderPassword { get; }
    }
}