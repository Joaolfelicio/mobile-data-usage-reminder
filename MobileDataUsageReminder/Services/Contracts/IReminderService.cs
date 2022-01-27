using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;

namespace MobileDataUsageReminder.Services.Contracts
{
    public interface IReminderService
    {
        /// <summary>
        /// Sends the reminder.
        /// </summary>
        /// <param name="dataUsages">The data usages.</param>
        Task SendReminder(List<MobileData> dataUsages);
    }
}