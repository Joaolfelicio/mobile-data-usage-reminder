using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class ClientResult
    {
        [JsonProperty("partyRole")]
        public PartyRole PartyRole { get; set; }
    }
}