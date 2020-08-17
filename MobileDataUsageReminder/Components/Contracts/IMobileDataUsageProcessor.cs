using System.Threading.Tasks;

namespace MobileDataUsageReminder.Components.Contracts
{
    public interface IMobileDataUsageProcessor
    {
        Task ProcessMobileDataUsage();
    }
}