using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumption
    {
        [JsonProperty("internalName")]
        public string Name { get; set; }
        [JsonProperty("amount")]
        public Amount Amount { get; set; }
    }
}