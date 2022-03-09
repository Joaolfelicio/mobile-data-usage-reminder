public class MapperService : IMapperService
{
    public MobileData MapMobileData(DataUsage dataUsage)
    {
        string truncatedUsedAmount;
        if (dataUsage.UsedAmount.Contains('.'))
        {
            truncatedUsedAmount = dataUsage.UsedAmount[..dataUsage.UsedAmount.IndexOf('.', StringComparison.Ordinal)];
        }
        else
        {
            truncatedUsedAmount = dataUsage.UsedAmount;
        }

        var usedPercentage = int.Parse(truncatedUsedAmount) * 100 / int.Parse(dataUsage.InitialAmount);

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
            UsedAmount = dataUsage.UsedAmount,
            RemainingAmount = dataUsage.RemainingAmount,
            ChatId = dataUsage.TelegramUser?.ChatId,
            UsedPercentage = roundedUsedPercentage
        };
    }
}
