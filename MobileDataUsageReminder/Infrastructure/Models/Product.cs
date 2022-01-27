using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productOfferingId")]
        public string PackageId { get; set; }

        [JsonProperty("describedBy")]
        public List<Description> Descriptions { get; set; }
    }
}