using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class PartyRole
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}