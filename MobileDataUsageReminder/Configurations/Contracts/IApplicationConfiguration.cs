namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface IApplicationConfiguration
    {
        string RecordsFileName { get; }
        string ProviderEmail { get; }
        string ProviderPassword { get; }
    }
}