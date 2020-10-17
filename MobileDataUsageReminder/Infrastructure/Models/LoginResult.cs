using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class LoginResult
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("access_token")]
        public string TokenValue { get; set; }
    }
}