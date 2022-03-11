using System.Text.Json.Serialization;

internal record DataConsumptionResult([property:JsonPropertyName("results")] List<DataConsumption> DataConsumptions);

internal record DataConsumption([property:JsonPropertyName("internalName")] string Name, [property:JsonPropertyName("amount")] Amount Amount);

internal record Amount([property:JsonPropertyName("unit")] string Unit, [property:JsonPropertyName("initial")] int InitialAmount, [property:JsonPropertyName("used")] float UsedAmount, [property:JsonPropertyName("remaining")] float RemainingAmount);

[JsonSerializable(typeof(DataConsumptionResult))]
internal partial class DataConsumptionResultContext : JsonSerializerContext {}
