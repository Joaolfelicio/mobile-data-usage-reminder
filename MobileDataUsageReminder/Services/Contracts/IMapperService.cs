using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IMapperService
    {
        MobileData MapMobileData(DataUsage dataUsage);
    }
}
