using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using MobileDataUsageReminder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MobileDataUsageReminder.Components;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Services.Contracts;
using Serilog;

namespace MobileDataUsageReminder
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }
        private static IMobileDataUsageProcessor MobileDataUsageProcessor { get; set; }

        static void Main(string[] args)
        {
            Configuration = StartUp();

            StartLogger(Configuration);

            var servicesCollection = new ServiceCollection();
            ConfigureServices(servicesCollection);

            ServiceProvider = servicesCollection.BuildServiceProvider();


            try
            {
                MobileDataUsageProcessor = ServiceProvider.GetRequiredService<IMobileDataUsageProcessor>();

                MobileDataUsageProcessor.ProcessMobileDataUsage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            services.Configure<MobileDataConfiguration>(Configuration.GetSection(nameof(MobileDataConfiguration)))
                    .AddSingleton<IMobileDataConfiguration>(sp => sp.GetRequiredService<IOptions<MobileDataConfiguration>>().Value)
                    .AddScoped<IMobileDataUsageProcessor, MobileDataUsageProcessor>()
                    .AddScoped<IProviderDataUsage, OrangeDataUsage>();
        }
    }
}
