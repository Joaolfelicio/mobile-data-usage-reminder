using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ProductsResult
{
    [JsonPropertyName("results")]
    public List<Product> Products { get; init; }
}
