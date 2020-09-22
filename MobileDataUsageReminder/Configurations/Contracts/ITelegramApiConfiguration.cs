namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface ITelegramApiConfiguration
    {
        int ChatId { get; }
        string ApiEndPoint { get; }
        string AccessToken { get; }
    }
}