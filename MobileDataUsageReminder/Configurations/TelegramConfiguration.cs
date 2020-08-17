using MobileDataUsageReminder.Configurations.Contracts;

namespace MobileDataUsageReminder.Configurations
{
    public class TelegramConfiguration : ITelegramConfiguration
    {
        public int ChatId { get; set; }
        public string ApiEndPoint { get; set; }
    }
}