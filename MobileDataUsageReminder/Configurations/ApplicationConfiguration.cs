using MobileDataUsageReminder.Configurations.Contracts;

namespace MobileDataUsageReminder.Configurations
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string Test { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderPassword { get; set; }
    }
}