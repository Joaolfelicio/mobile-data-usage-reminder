using System;
using System.Net.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

[assembly: FunctionsStartup(typeof(FunctionStartup))]
public class FunctionStartup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        builder.Services.AddOptions<ApplicationConfiguration>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection(nameof(ApplicationConfiguration)).Bind(settings);
               });

        builder.Services.AddOptions<TelegramApiConfiguration>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   configuration.GetSection(nameof(TelegramApiConfiguration)).Bind(settings);
               });

        builder.Services
               .AddSingleton<IApplicationConfiguration>(sp =>
                    sp.GetRequiredService<IOptions<ApplicationConfiguration>>().Value)
               .AddSingleton<ITelegramApiConfiguration>(sp =>
                    sp.GetRequiredService<IOptions<TelegramApiConfiguration>>().Value)
               .AddSingleton<IMongoContext>(sp =>
               {
                   var mongoConfig = configuration.GetSection(nameof(MongoConfiguration))
                                                  .Get<MongoConfiguration>();

                   return new MongoContext(mongoConfig);
               })
               .AddScoped<IProviderDataUsageService, ProviderDataUsageService>()
               .AddScoped<IOrangeConstants, OrangeConstants>()
               .AddScoped<IOrangeEndpoints, OrangeEndpoints>()
               .AddScoped<IFilterService, FilterService>()
               .AddScoped<IReminderService, ReminderService>()
               .AddScoped<IMobileDataRepository, MobileDataRepository>()
               .AddScoped<IMapperService, MapperService>();

        builder.Services.AddHttpClient<INotificationGateway, TelegramGateway>()
               .SetHandlerLifetime(TimeSpan.FromMinutes(5))
               .AddPolicyHandler(RetryPolicy());

        builder.Services.AddHttpClient<IDataProviderGateway, OrangeGateway>()
               .SetHandlerLifetime(TimeSpan.FromMinutes(5))
               .AddPolicyHandler(RetryPolicy());
    }

    private IAsyncPolicy<HttpResponseMessage> RetryPolicy()
    {
        var jitterer = new Random();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => 
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)));
    }
}
