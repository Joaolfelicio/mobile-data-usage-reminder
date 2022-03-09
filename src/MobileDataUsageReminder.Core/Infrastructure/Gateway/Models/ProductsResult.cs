using System.Text.Json.Serialization;

//TODO: Check the camel case difference from record and json

internal record ProductsResult([property:JsonPropertyName("results")] List<Product> Products);

internal record Product([property:JsonPropertyName("id")] string Id, [property:JsonPropertyName("productOfferingId")] string PackageId, [property:JsonPropertyName("describedBy")] List<Description> Descriptions);

internal record Description([property:JsonPropertyName("name")] string Name, [property:JsonPropertyName("currentValue")] CurrentValue CurrentValue);

internal record CurrentValue([property:JsonPropertyName("value")] string Value);

[JsonSerializable(typeof(ProductsResult))]
internal partial class ProductsResultContext : JsonSerializerContext {}
