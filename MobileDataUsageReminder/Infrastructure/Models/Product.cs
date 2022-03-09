using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("productOfferingId")]
        public string PackageId { get; set; }

        [JsonPropertyName("describedBy")]
        public List<Description> Descriptions { get; set; }
    }
}