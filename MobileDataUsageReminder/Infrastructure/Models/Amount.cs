using System.Text.Json.Serialization;

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
