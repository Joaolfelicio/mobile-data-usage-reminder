using System.Text.Json.Serialization;

//TODO: Why is it using json property name? this should not be a dto
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
