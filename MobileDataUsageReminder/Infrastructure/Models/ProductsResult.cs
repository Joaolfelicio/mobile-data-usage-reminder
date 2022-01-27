using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ProductsResult
    {
        [JsonProperty("results")]
        public List<Product> Products { get; set; }
    }
}