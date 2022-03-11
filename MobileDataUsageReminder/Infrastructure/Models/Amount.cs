using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Amount
    {
        [JsonProperty("unit")]
        public string Unit { get; set; }
        [JsonProperty("initial")]
        public string InitialAmount { get; set; }
        [JsonProperty("used")]
        public string UsedAmount { get; set; }
        [JsonProperty("remaining")]
        public string RemainingAmount { get; set; }
    }
}