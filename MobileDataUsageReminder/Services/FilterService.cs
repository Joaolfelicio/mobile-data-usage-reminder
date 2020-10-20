using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Services.Contracts;

namespace MobileDataUsageReminder.Services
{
    public class FilterService : IFilterService
    {
        private readonly IMobileDataRepository _mobileDataRepository;
        private readonly ILogger<FilterService> _logger;

        public FilterService(IMobileDataRepository mobileDataRepository,
            ILogger<FilterService> logger)
        {
            _mobileDataRepository = mobileDataRepository;
            _logger = logger;
        }
        public List<MobileData> FilterNewMobileDatas(List<MobileData> mobileDatas)
        {
            var newMobileData = new List<MobileData>();

            foreach (var mobileData in mobileDatas)
            {
                var hasReminderSent = _mobileDataRepository.HasReminderAlreadySent(mobileData);

                if (hasReminderSent == false)
                {
                    newMobileData.Add(mobileData);
                    _logger.LogInformation($"Reminder will be sent for {mobileData.PhoneNumber}, reached {mobileData.UsedPercentage}% in {mobileData.Month}");
                }
                else
                {
                    _logger.LogInformation($"Reminder was already sent for {mobileData.PhoneNumber}, reached {mobileData.UsedPercentage}% in {mobileData.Month}");
                }
            }

            return newMobileData;
        }
    }
}