using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Components
{
    public class MobileDataUsageProcessor : IMobileDataUsageProcessor
    {
        private readonly IProviderDataUsage _providerDataUsage;
        private readonly IPreviousRemindersService _previousRemindersService;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IReminderService _reminderService;

        public MobileDataUsageProcessor(IProviderDataUsage providerDataUsage,
                                        IPreviousRemindersService previousRemindersService,
                                        IApplicationConfiguration applicationConfiguration,
                                        IReminderService reminderService)
        {
            _providerDataUsage = providerDataUsage;
            _previousRemindersService = previousRemindersService;
            _applicationConfiguration = applicationConfiguration;
            _reminderService = reminderService;
        }

        public async Task ProcessMobileDataUsage()
        {
            //Get current mobile usage
            var mobileDataUsages = _providerDataUsage.GetMobileDataUsage();

            //Archive (if they exist) previous year records
            _previousRemindersService.ArchivePreviousYearReminders(_applicationConfiguration.RecordsFileName);

            //Get all the previous data usage records
            var previousReminders = _previousRemindersService.GetAllDataUsages(_applicationConfiguration.RecordsFileName);

            //Build a list with the new reminders to be send
            var remindersToSend = _previousRemindersService.DataUsagesToRemind(previousReminders, mobileDataUsages);

            //Concat the new reminders plus the previous reminders
            var allReminders = previousReminders.Concat(remindersToSend).ToList();

            //Write the full list reminder to a file
            _previousRemindersService.WriteAllDataUsages(_applicationConfiguration.RecordsFileName, allReminders);

            if (remindersToSend.Count > 0)
            {
                //Send reminder via reminder service
                await _reminderService.SendReminder(remindersToSend);
            }
        }
    }
}