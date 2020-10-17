using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ProductsResult
    {
        [JsonProperty("results")]
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productOfferingId")]
        public string PackageId { get; set; }

        [JsonProperty("describedBy")]
        public List<Description> Descriptions { get; set; }
    }

    public class Description
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currentValue")]
        public CurrentValue CurrentValue { get; set; }
    }

    public class CurrentValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}