using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class PartyRole
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}