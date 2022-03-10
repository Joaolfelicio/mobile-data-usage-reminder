public class MapperService : IMapperService
{
    public MobileData MapMobileData(DataUsage dataUsage)
    {
        var usedAmount = (int)dataUsage.UsedAmount;

        var usedPercentage = usedAmount * 100 / dataUsage.InitialAmount;

        var roundedUsedPercentage = Convert.ToInt32(Math.Round(usedPercentage / 10.0, MidpointRounding.AwayFromZero) * 10);

        return new MobileData
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
        };
    }
}
