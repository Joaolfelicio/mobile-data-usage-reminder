using System.Collections.Generic;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IFilterService
    {
        List<MobileData> FilterNewMobileDatas(List<MobileData> mobileDatas);
    }
}