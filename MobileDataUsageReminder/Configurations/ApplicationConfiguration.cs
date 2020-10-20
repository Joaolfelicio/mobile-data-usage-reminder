using MobileDataUsageReminder.Configurations.Contracts;

namespace MobileDataUsageReminder.Configurations
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string ProviderEmail { get; set; }
        public string ProviderPassword { get; set; }
    }
}