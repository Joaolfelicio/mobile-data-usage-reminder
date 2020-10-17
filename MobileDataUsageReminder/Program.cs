using System;
using System.IO;
using System.Reflection;
using MobileDataUsageReminder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MobileDataUsageReminder.Components;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Scheduler;
using MobileDataUsageReminder.Services.Contracts;
using Serilog;
using ApplicationConfiguration = MobileDataUsageReminder.Configurations.ApplicationConfiguration;

namespace MobileDataUsageReminder
{
    static class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }
        
        [STAThread]
        static void Main(string[] args)
        {
            Configuration = StartUp();

            StartLogger(Configuration);

            var servicesCollection = new ServiceCollection();
            ConfigureServices(servicesCollection);

            ServiceProvider = servicesCollection.BuildServiceProvider();

            var jobScheduler = new JobScheduler(ServiceProvider);
            jobScheduler.Run().GetAwaiter().GetResult();
        }

        static IConfiguration StartUp()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        static void StartLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationConfiguration>(Configuration.GetSection(nameof(ApplicationConfiguration)))
                .AddSingleton<IApplicationConfiguration>(sp =>
                    sp.GetRequiredService<IOptions<ApplicationConfiguration>>().Value)
                .Configure<TelegramApiConfiguration>(Configuration.GetSection(nameof(TelegramApiConfiguration)))
                .AddSingleton<ITelegramApiConfiguration>(sp =>
                    sp.GetRequiredService<IOptions<TelegramApiConfiguration>>().Value)
                .AddScoped<IMobileDataUsageProcessor, MobileDataUsageProcessor>()
                .AddScoped<IProviderDataUsage, OrangeDataUsage>()
                .AddScoped<IPreviousRemindersService, PreviousRemindersService>()
                .AddScoped<IReminderGateway, TelegramGateway>()
                .AddScoped<IReminderService, ReminderService>()
                .AddScoped<DataUsageReminderJob>();

            services.AddLogging(l =>
            {
                l.AddSerilog(logger: Log.Logger, dispose: true);
            });
        }
    }
}
