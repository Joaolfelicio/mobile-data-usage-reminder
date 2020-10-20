using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.DAL.Repository.Contracts
{
    public interface IMobileDataRepository
    {
        bool HasReminderAlreadySent(MobileData mobileData);

        Task CreateMobileDatas(List<MobileData> mobileDatas);
    }
}