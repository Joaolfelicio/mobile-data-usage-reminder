using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IProviderDataUsageService
    {
        Task<List<MobileData>> GetMobileData();
    }
}
