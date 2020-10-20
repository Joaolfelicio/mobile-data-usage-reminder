using System.Collections.Generic;
using System.Threading.Tasks;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderGateway _reminderGateway;

        public ReminderService(IReminderGateway reminderGateway)
        {
            _reminderGateway = reminderGateway;
        }
        /// <summary>
        /// Sends the reminder.
        /// </summary>
        /// <param name="dataUsages">The data usages.</param>
        public async Task SendReminder(List<MobileData> dataUsages)
        {
            foreach (var dataUsage in dataUsages)
            {
                await _reminderGateway.SendPostToApiReminder(dataUsage);
            }
        }
    }
}