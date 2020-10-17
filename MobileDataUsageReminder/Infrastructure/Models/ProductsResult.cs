using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ProductsResult
    {
        [JsonPropertyName("results")]
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }

        [JsonPropertyName("productOfferingId")]
        public string PackageId { get; set; }

        [JsonPropertyName("describedBy")]
        public List<Description> Descriptions { get; set; }
    }

    public class Description
    {
        public string Name { get; set; }

        public CurrentValue CurrentValue { get; set; }
    }

    public class CurrentValue
    {
        public string Value { get; set; }
    }
}