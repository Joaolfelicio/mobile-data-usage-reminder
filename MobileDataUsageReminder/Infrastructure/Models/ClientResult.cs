using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ClientResult
    {
        [JsonProperty("partyRole")]
        public PartyRole PartyRole { get; set; }
    }

    public class PartyRole
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}