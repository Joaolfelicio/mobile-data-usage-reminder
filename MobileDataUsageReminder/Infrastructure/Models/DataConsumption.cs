using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumption
    {
        [JsonPropertyName("internalName")]
        public string Name { get; set; }
        [JsonPropertyName("amount")]
        public Amount Amount { get; set; }
    }
}