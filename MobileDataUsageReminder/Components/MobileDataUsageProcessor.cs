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
            var mobileDataUsages = _providerDataUsage.GetMobileDataUsage();

            _previousRemindersService.ArchivePreviousYearReminders(_applicationConfiguration.RecordsFileName);

            var previousReminders = _previousRemindersService.GetAllDataUsages(_applicationConfiguration.RecordsFileName);

            var remindersToSend = _previousRemindersService.DataUsagesToRemind(previousReminders, mobileDataUsages);

            var allReminders = previousReminders.Concat(remindersToSend).ToList();

            _previousRemindersService.WriteAllDataUsages(_applicationConfiguration.RecordsFileName, allReminders);

            if (remindersToSend.Count > 0)
            {
                await _reminderService.SendReminder(remindersToSend);
            }
        }
    }
}