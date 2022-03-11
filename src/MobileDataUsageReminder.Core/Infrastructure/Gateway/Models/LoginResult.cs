using System.Text.Json.Serialization;

internal record LoginResult([property:JsonPropertyName("token_type")] string TokenType, [property:JsonPropertyName("access_token")] string TokenValue);

[JsonSerializable(typeof(LoginResult))]
internal partial class LoginResultContext : JsonSerializerContext {}