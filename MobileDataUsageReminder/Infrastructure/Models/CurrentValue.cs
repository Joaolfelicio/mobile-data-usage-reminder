using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class CurrentValue
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}