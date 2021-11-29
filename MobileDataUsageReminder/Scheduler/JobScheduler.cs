﻿using System;
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
                var triggerConfig = TriggerBuilder.Create()
                    .WithIdentity("MobileDataUsageReminderTrigger", "MobileDataUsageReminderJob")
                    .StartNow();

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    triggerConfig.WithSimpleSchedule(x => x
                        .RepeatForever()
                        .WithIntervalInSeconds(20));
                }
                else
                {
                    triggerConfig.WithDailyTimeIntervalSchedule(x => x
                        .WithIntervalInMinutes(30)
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 0))
                        .OnEveryDay());
                }

                var trigger = triggerConfig.Build();

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