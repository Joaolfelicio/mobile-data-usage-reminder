using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IFilterService
    {
        Task<List<MobileData>> FilterNewMobileDatas(List<MobileData> mobileDatas);
    }
}