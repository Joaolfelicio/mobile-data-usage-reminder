using System.Collections.Generic;

namespace MobileDataUsageReminder.Configurations.Contracts
{
    public interface ITelegramApiConfiguration
    {
        public List<TelegramUser> TelegramUsers { get; set; }
        string ApiEndPoint { get; }
        string AccessToken { get; }
    }
}