using System.Text.Json.Serialization;

public record EventPayload([property: JsonPropertyName("update_id")] int Id, Message Message);

public record Message([property: JsonPropertyName("message_id")] int Id, User From, int Date, [property: JsonPropertyName("text")] string CommandType);

public record User(int Id, string Username);

[JsonSerializable(typeof(EventPayload))]
internal partial class EventPayloadContext : JsonSerializerContext { }
