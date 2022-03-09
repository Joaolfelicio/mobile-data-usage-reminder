using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class MobileDataUsageReminderFunction
{
    private readonly IProviderDataUsageService _providerDataUsage;
    private readonly IReminderService _reminderService;
    private readonly ILogger<MobileDataUsageReminderFunction> _logger;
    private readonly IMobileDataRepository _mobileDataRepository;
    private readonly IFilterService _filterService;

    public MobileDataUsageReminderFunction(
        IProviderDataUsageService providerDataUsage,
        IReminderService reminderService,
        ILogger<MobileDataUsageReminderFunction> logger,
        IMobileDataRepository mobileDataRepository,
        IFilterService filterService)
    {
        _providerDataUsage = providerDataUsage;
        _reminderService = reminderService;
        _logger = logger;
        _mobileDataRepository = mobileDataRepository;
        _filterService = filterService;
    }

    [FunctionName(nameof(MobileDataUsageReminderFunction))]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timeTrigger)
    {
        var mobileData = await _providerDataUsage.GetMobileData();

        var newMobileDatas = _filterService.FilterNewMobileDatas(mobileData);

        if (newMobileDatas.Count > 0)
        {
            _logger.LogInformation("There are {count} reminders to be sent.", newMobileDatas.Count);

            var reminderTask = _reminderService.SendReminder(newMobileDatas);

            var createDataTask = _mobileDataRepository.CreateMobileData(newMobileDatas);

            await Task.WhenAll(reminderTask, createDataTask);
        }
        else
        {
            _logger.LogInformation("There are no reminders to be sent.");
        }
    }
}
