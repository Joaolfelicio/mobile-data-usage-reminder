using Newtonsoft.Json;

namespace MobileDataUsageReminder.Models
{
    public class ProviderCredentials
    {
        public ProviderCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [JsonProperty("username")]
        public string Username { get; }

        [JsonProperty("password")]
        public string Password { get; }
    }
}
