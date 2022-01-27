using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumptionResult
    {
        [JsonProperty("results")]
        public List<DataConsumption> DataConsumptions { get; set; }
    }
}