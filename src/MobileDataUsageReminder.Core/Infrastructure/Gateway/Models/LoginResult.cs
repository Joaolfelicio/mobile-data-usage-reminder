using System.Text.Json.Serialization;

public class LoginResult
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }
    [JsonPropertyName("access_token")]
    public string TokenValue { get; init; }
}
