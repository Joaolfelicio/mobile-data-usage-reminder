using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IProviderDataUsage
    {
        /// <summary>
        /// Gets the mobile data packages.
        /// </summary>
        /// <returns></returns>
        Task<List<MobileData>> GetMobileData();
    }
}
