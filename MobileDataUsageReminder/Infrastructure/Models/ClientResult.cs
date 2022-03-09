using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ClientResult
    {
        [JsonPropertyName("partyRole")]
        public PartyRole PartyRole { get; set; }
    }
}