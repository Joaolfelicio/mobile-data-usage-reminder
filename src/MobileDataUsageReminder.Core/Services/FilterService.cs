using Microsoft.Extensions.Logging;

public class FilterService : IFilterService
{
    private readonly IMobileDataRepository _mobileDataRepository;
    private readonly ILogger<FilterService> _logger;

    public FilterService(
        IMobileDataRepository mobileDataRepository,
        ILogger<FilterService> logger)
    {
        _mobileDataRepository = mobileDataRepository;
        _logger = logger;
    }

    public IEnumerable<MobileData> FilterNewMobileDatas(IEnumerable<MobileData> mobileDatas)
    {
        var newMobileData = new List<MobileData>();

        foreach (var mobileData in mobileDatas)
        {
            var hasReminderSent = _mobileDataRepository.WasReminderAlreadySent(mobileData);

            if (!hasReminderSent && mobileData.UsedPercentage > 0)
            {
                newMobileData.Add(mobileData);
                _logger.LogInformation("Reminder will be sent for {phone number}, reached {used percentage}% in {month}", mobileData.PhoneNumber, mobileData.UsedPercentage, mobileData.Month);
            }
            else
            {
                _logger.LogInformation("Reminder was already sent for {phone number} or it's at 0%, reached {used percentage}% in {month}", mobileData.PhoneNumber, mobileData, mobileData.Month);
            }
        }

        return newMobileData;
    }
}
