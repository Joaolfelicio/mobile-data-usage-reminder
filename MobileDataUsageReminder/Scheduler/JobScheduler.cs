using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;

namespace MobileDataUsageReminder.Scheduler
{
    public class JobScheduler
    {
        private readonly IServiceProvider _serviceProvider;

        public JobScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Run()
        {
            try
            {
                // Grab the scheduler instance from the factory.
                var props = new NameValueCollection()
                {
                    {"quartz.serializer.type", "binary"}
                };

                LogProvider.IsDisabled = true;

                var factory = new StdSchedulerFactory(props);
                var scheduler = await factory.GetScheduler();

                scheduler.JobFactory = new JobFactory(_serviceProvider);

                // And start it off
                await scheduler.Start();

                // Define the job and tie it to our job
                var job = JobBuilder.Create<DataUsageReminderJob>()
                    .WithIdentity("MobileDataUsageReminderJob")
                    .Build();

                // Trigger the job to run now and then repeat every x minutes
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("MobileDataUsageReminderTrigger", "MobileDataUsageReminderJob")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(30)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

                // -1 so it never stops
                await Task.Delay(-1);

                // And last shut down the scheduler when you are ready to close your program
                await scheduler.Shutdown();
            }
            catch (SchedulerException exception)
            {
                await Console.Error.WriteLineAsync(exception.ToString());
            }
        }
    }

}