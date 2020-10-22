using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Components
{
    public class MobileDataUsageProcessor : IMobileDataUsageProcessor
    {
        private readonly IProviderDataUsage _providerDataUsage;
        private readonly IReminderService _reminderService;
        private readonly ILogger<MobileDataUsageProcessor> _logger;
        private readonly IMobileDataRepository _mobileDataRepository;
        private readonly IFilterService _filterService;

        public MobileDataUsageProcessor(IProviderDataUsage providerDataUsage,
                                        IReminderService reminderService,
                                        ILogger<MobileDataUsageProcessor> logger,
                                        IMobileDataRepository mobileDataRepository,
                                        IFilterService filterService)
        {
            _providerDataUsage = providerDataUsage;
            _reminderService = reminderService;
            _logger = logger;
            _mobileDataRepository = mobileDataRepository;
            _filterService = filterService;
        }

        public async Task ProcessMobileDataUsage()
        {
            // Get current mobile usage
            var mobileData = await _providerDataUsage.GetMobileData();

            // Filter the mobile datas, so we can only keep the new ones
            var newMobileDatas = _filterService.FilterNewMobileDatas(mobileData);

            if (newMobileDatas.Count > 0)
            {
                _logger.LogInformation($"There are {newMobileDatas.Count} reminders to be sent");

                // Send reminder via reminder service
                await _reminderService.SendReminder(newMobileDatas);

                // Update the db with the reminders that are going to be sent
                await _mobileDataRepository.CreateMobileDatas(newMobileDatas);
            }
            else
            {
                _logger.LogInformation($"There are no reminders to be sent");
            }
        }
    }
}