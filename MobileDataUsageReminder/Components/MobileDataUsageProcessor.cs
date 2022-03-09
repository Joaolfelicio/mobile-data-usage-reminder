using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Components
{
    public class MobileDataUsageProcessor : IMobileDataUsageProcessor
    {
        private readonly IProviderDataUsageService _providerDataUsage;
        private readonly IReminderService _reminderService;
        private readonly ILogger<MobileDataUsageProcessor> _logger;
        private readonly IMobileDataRepository _mobileDataRepository;
        private readonly IFilterService _filterService;

        public MobileDataUsageProcessor(  
            IProviderDataUsageService providerDataUsage,
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
                _logger.LogInformation("There are {count} reminders to be sent.", newMobileDatas.Count);

                var reminderTask = _reminderService.SendReminder(newMobileDatas);


                var createDataTask = _mobileDataRepository.CreateMobileDatas(newMobileDatas);

                await Task.WhenAll(reminderTask, createDataTask);
            }
            else
            {
                _logger.LogInformation("There are no reminders to be sent.");
            }
        }
    }
}