using System.Collections.Generic;
using System.Text.Json.Serialization;

public class DataConsumptionResult
{
    [JsonPropertyName("results")]
    public List<DataConsumption> DataConsumptions { get; init; }
}
