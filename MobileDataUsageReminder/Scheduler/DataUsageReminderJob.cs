using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Components.Contracts;
using Quartz;

namespace MobileDataUsageReminder.Scheduler
{
    public class DataUsageReminderJob : IJob
    {
        private readonly IMobileDataUsageProcessor _mobileDataUsageProcessor;
        private readonly ILogger<DataUsageReminderJob> _logger;

        public DataUsageReminderJob(
            IMobileDataUsageProcessor mobileDataUsageProcessor,
            ILogger<DataUsageReminderJob> logger)
        {
            _mobileDataUsageProcessor = mobileDataUsageProcessor;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Starting mobile data usage process.");

                await _mobileDataUsageProcessor.ProcessMobileDataUsage();

                _logger.LogInformation("Finished mobile data usage process.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}