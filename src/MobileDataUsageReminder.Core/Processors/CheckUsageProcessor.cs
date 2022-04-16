public class CheckUsageProcessor : ICheckUsageProcessor
{
    private readonly IProviderDataUsageService _providerDataUsage;
    private readonly IMapperService _mapperService;
    private readonly IReminderService _reminderService;

    public CheckUsageProcessor(
        IProviderDataUsageService providerDataUsage,
        IMapperService mapperService,
        IReminderService reminderService)
    {
        _providerDataUsage = providerDataUsage;
        _mapperService = mapperService;
        _reminderService = reminderService;
    }

    public async Task ProcessCommand(CommandEventPayload eventPayload)
    {
        bool IsSameFromId(DataUsage du) => du.TelegramUser.ChatId == eventPayload.Message.From.Id.ToString();

        var dataUsage = (await _providerDataUsage.GetDataUsage()).First(IsSameFromId);

        var mobileData = _mapperService.MapMobileData(new List<DataUsage> { dataUsage });

        await _reminderService.SendReminders(mobileData.ToList());
    }
}
