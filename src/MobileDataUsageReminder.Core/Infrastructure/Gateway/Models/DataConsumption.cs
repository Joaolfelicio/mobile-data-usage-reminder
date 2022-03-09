using System.Text.Json.Serialization;
using MobileDataUsageReminder.Infrastructure.Models;

public class DataConsumption
{
    [JsonPropertyName("internalName")]
    public string Name { get; init; }
    [JsonPropertyName("amount")]
    public Amount Amount { get; init; }
}
