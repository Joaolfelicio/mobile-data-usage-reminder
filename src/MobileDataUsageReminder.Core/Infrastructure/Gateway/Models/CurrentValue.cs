using System.Text.Json.Serialization;

public class CurrentValue
{
    [JsonPropertyName("value")]
    public string Value { get; init; }
}
