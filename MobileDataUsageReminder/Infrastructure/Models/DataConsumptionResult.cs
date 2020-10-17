using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumptionResult
    {
        [JsonProperty("results")]
        public List<DataConsumption> DataConsumptions { get; set; }
    }

    public class DataConsumption
    {
        [JsonProperty("internalName")]
        public string Name { get; set; }
        [JsonProperty("amount")]
        public Amount Amount { get; set; }
    }

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