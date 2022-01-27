using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Description
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currentValue")]
        public CurrentValue CurrentValue { get; set; }
    }
}