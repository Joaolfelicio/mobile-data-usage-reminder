using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class DataConsumptionResult
    {
        [JsonPropertyName("results")]
        public List<DataConsumption> DataConsumptions { get; set; }
    }
}