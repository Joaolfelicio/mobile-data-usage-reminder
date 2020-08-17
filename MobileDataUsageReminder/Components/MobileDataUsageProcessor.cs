using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Components
{
    public class MobileDataUsageProcessor : IMobileDataUsageProcessor
    {
        private readonly IProviderDataUsage _providerDataUsage;

        public MobileDataUsageProcessor(IProviderDataUsage providerDataUsage)
        {
            _providerDataUsage = providerDataUsage;
        }
        public void ProcessMobileDataUsage()
        {
            var mobileDataUsages = _providerDataUsage.GetMobileDataUsage();
        }
    }
}