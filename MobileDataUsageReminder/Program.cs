using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MobileDataUsageReminder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MobileDataUsageReminder.Components;
using MobileDataUsageReminder.Components.Contracts;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Constants;
using MobileDataUsageReminder.Constants.Contracts;
using MobileDataUsageReminder.DAL.DataContext;
using MobileDataUsageReminder.DAL.Repository;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Infrastructure;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Scheduler;
using MobileDataUsageReminder.Services.Contracts;
using Npgsql;
using Serilog;
using ApplicationConfiguration = MobileDataUsageReminder.Configurations.ApplicationConfiguration;
using System.Net.Http;
using Polly.Extensions.Http;
using Polly.Contrib.WaitAndRetry;
using Polly;

namespace MobileDataUsageReminder
{
    static class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }
        private static string EnvironmentName { get; set; }

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
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", true, true)
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
                .AddScoped<IProviderDataUsageService, ProviderDataUsageService>()
                .AddScoped<IOrangeConstants, OrangeConstants>()
                .AddScoped<IOrangeEndpoints, OrangeEndpoints>()
                .AddScoped<IFilterService, FilterService>()
                .AddScoped<IReminderService, ReminderService>()
                .AddScoped<IMobileDataRepository, MobileDataRepository>()
                .AddScoped<IMapperService, MapperService>()
                .AddScoped<DataUsageReminderJob>();

            services.AddHttpClient<IReminderGateway, TelegramGateway>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IProviderGateway, OrangeGateway>()
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddLogging(l =>
            {
                l.AddSerilog(Log.Logger, true);
            });

            // Setup database connection string
            string connectionString = Configuration.GetConnectionString("MobileDataUsageConnectionString");

            if (EnvironmentName != "Development" && string.IsNullOrWhiteSpace(connectionString))
            {
                // Get connection string from Heroku Postgresql
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(databaseUrl ?? string.Empty);
                var userInfo = databaseUri.UserInfo.Split(':');

                var npgsqlBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/'),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };

                connectionString = npgsqlBuilder.ToString();
            }

            services.AddDbContext<MobileDataUsageContext>(options => options.UseNpgsql(connectionString, builder => 
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
        }
    }
}
