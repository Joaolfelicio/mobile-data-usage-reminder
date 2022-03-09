using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Models
{
    public class ProviderCredentials
    {
        public ProviderCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [JsonPropertyName("username")]
        public string Username { get; }

        [JsonPropertyName("password")]
        public string Password { get; }
    }
}
