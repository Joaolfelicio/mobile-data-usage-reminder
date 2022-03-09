using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Amount
    {
        [JsonPropertyName("unit")]
        public string Unit { get; init; }
        [JsonPropertyName("initial")]
        public string InitialAmount { get; init; }
        [JsonPropertyName("used")]
        public string UsedAmount { get; init; }
        [JsonPropertyName("remaining")]
        public string RemainingAmount { get; init; }
    }
}