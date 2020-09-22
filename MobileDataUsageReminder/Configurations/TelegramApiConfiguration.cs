using MobileDataUsageReminder.Configurations.Contracts;

namespace MobileDataUsageReminder.Configurations
{
    public class TelegramApiConfiguration : ITelegramApiConfiguration
    {
        public int ChatId { get; set; }
        public string ApiEndPoint { get; set; }
        public string AccessToken { get; set; }
    }
}