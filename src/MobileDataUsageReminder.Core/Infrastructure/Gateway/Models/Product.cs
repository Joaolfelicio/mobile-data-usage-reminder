using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Product
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("productOfferingId")]
    public string PackageId { get; init; }

    [JsonPropertyName("describedBy")]
    public List<Description> Descriptions { get; init; }
}
