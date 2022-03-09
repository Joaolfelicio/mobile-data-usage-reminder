using System.Text.Json.Serialization;

public class Description
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("currentValue")]
    public CurrentValue CurrentValue { get; init; }
}
