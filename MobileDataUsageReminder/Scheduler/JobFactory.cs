using System;
using Microsoft.Extensions.DependencyInjection;
using MobileDataUsageReminder.Components.Contracts;
using Quartz;
using Quartz.Spi;

namespace MobileDataUsageReminder.Scheduler
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceProvider.GetRequiredService<DataUsageReminderJob>();
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}