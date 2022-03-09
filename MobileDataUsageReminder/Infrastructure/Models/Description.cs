using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Description
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("currentValue")]
        public CurrentValue CurrentValue { get; set; }
    }
}