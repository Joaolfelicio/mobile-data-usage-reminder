using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class LoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}