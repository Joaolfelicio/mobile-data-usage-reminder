using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class MobileDataUsageReminderFunction
{
    private readonly IProviderDataUsageService _providerDataUsage;
    private readonly IMapperService _mapperService;
    private readonly IReminderService _reminderService;
    private readonly ILogger<MobileDataUsageReminderFunction> _logger;
    private readonly IMobileDataRepository _mobileDataRepository;
    private readonly IFilterService _filterService;

    public MobileDataUsageReminderFunction(
        IProviderDataUsageService providerDataUsage,
        IMapperService mapperService,
        IReminderService reminderService,
        ILogger<MobileDataUsageReminderFunction> logger,
        IMobileDataRepository mobileDataRepository,
        IFilterService filterService)
    {
        _providerDataUsage = providerDataUsage;
        _mapperService = mapperService;
        _reminderService = reminderService;
        _logger = logger;
        _mobileDataRepository = mobileDataRepository;
        _filterService = filterService;
    }


    //TODO: Add timer as config
    [FunctionName(nameof(MobileDataUsageReminderFunction))]
    public async Task Run([TimerTrigger("%TimerSchedule%", RunOnStartup = true)] TimerInfo timer)
    {
        var dataUsage = await _providerDataUsage.GetDataUsage();

        var mobileData = _mapperService.MapMobileDataRoundUpPercent(dataUsage);

        var newMobileDatas = _filterService.FilterNewMobileDatas(mobileData);

        if (newMobileDatas.Any())
        {
            _logger.LogInformation("There are {count} reminders to be sent.", newMobileDatas.Count());

            var reminderTask = _reminderService.SendReminders(newMobileDatas.ToList());

            var createDataTask = _mobileDataRepository.CreateMobileData(newMobileDatas);

            await Task.WhenAll(reminderTask, createDataTask);
        }
        else
        {
            _logger.LogInformation("There are no reminders to be sent.");
        }
    }
}
