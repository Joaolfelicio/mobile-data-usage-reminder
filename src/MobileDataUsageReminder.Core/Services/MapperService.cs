public class MapperService : IMapperService
{
    public IEnumerable<MobileData> MapMobileData(IEnumerable<DataUsage> dataUsages) =>
        MapMobileData(dataUsages, (usedPercent) => usedPercent);

    public IEnumerable<MobileData> MapMobileDataRoundUpPercent(IEnumerable<DataUsage> dataUsages) =>
        MapMobileData(dataUsages, (usedPercent) => Math.Round(usedPercent / 10.0, MidpointRounding.AwayFromZero) * 10);

    private static IEnumerable<MobileData> MapMobileData(IEnumerable<DataUsage> dataUsages, Func<double, double> roundPercentMethod)
    {
        var mobileDatas = new List<MobileData>();

        foreach (var dataUsage in dataUsages)
        {
            var usedAmount = (int)dataUsage.UsedAmount;

            var usedPercentage = usedAmount * 100 / dataUsage.InitialAmount;

            var roundedUsedPercentage = roundPercentMethod(usedPercentage);

            mobileDatas.Add(new MobileData
            {
                PhoneNumber = dataUsage.TelegramUser?.PhoneNumber,
                FullDate = DateTime.Now,
                Day = DateTime.Now.Day,
                Month = DateTime.Now.ToString("MMMM"),
                Year = DateTime.Now.Year,
                Unit = dataUsage.Unit,
                InitialAmount = dataUsage.InitialAmount,
                UsedAmount = usedAmount,
                RemainingAmount = (int)dataUsage.RemainingAmount,
                ChatId = dataUsage.TelegramUser?.ChatId,
                UsedPercentage = roundedUsedPercentage
            });
        }

        return mobileDatas;
    }
}
