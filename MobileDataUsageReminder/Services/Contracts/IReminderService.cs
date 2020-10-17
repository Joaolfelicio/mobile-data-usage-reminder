using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IReminderService
    {
        Task SendReminder(List<MobileDataPackage> dataUsages);
    }
}