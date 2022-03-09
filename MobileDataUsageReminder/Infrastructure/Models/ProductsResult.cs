using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ProductsResult
    {
        [JsonPropertyName("results")]
        public List<Product> Products { get; set; }
    }
}