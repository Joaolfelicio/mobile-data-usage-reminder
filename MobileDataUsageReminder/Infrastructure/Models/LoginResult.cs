using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class LoginResult
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string TokenValue { get; set; }
    }
}