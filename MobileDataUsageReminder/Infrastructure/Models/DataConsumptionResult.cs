using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumptionResult
    {
        [JsonPropertyName("results")]
        public List<DataConsumption> DataConsumptions { get; set; }
    }

    public class DataConsumption
    {
        [JsonPropertyName("internalName")]
        public string Name { get; set; }
        public Amount Amount { get; set; }
    }

    public class Amount
    {
        [JsonPropertyName("unit")]
        public string Unit { get; set; }
        [JsonPropertyName("initial")]
        public string InitialAmount { get; set; }
        [JsonPropertyName("used")]
        public string UsedAmount { get; set; }
        [JsonPropertyName("remaining")]
        public string RemainingAmount { get; set; }
    }
}