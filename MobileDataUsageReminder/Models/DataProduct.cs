using MobileDataUsageReminder.Configurations;

namespace MobileDataUsageReminder.Models
{
    public class DataProduct
    {
        public string Id { get; set; }
        public string PackageId { get; set; }
        public TelegramUser TelegramUser { get; set; }
    }
}