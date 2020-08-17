namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface ITelegramConfiguration
    {
        int ChatId { get; }
        string ApiEndPoint { get; }
    }
}