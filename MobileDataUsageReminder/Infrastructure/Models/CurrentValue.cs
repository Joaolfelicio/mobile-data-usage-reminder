using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class CurrentValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}