using System.Text.Json.Serialization;

internal record DataConsumptionResult([property:JsonPropertyName("results")] List<DataConsumption> DataConsumptions);

internal record DataConsumption([property:JsonPropertyName("internalName")] string Name, [property:JsonPropertyName("internalName")] Amount Amount);

internal record Amount([property:JsonPropertyName("unit")] string Unit, [property:JsonPropertyName("initial")] string InitialAmount, [property:JsonPropertyName("used")] string UsedAmount, [property:JsonPropertyName("remaining")] string RemainingAmount);

[JsonSerializable(typeof(DataConsumptionResult))]
internal partial class DataConsumptionResultContext : JsonSerializerContext {}
