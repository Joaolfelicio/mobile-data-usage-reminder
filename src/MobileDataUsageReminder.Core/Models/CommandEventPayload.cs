using System.Text.Json.Serialization;

public record CommandEventPayload([property: JsonPropertyName("update_id")] int Id, Message Message);

public record Message([property: JsonPropertyName("message_id")] int Id, User From, int Date, [property: JsonPropertyName("text")] string CommandType);

public record User(int Id, string Username);

[JsonSerializable(typeof(CommandEventPayload))]
public partial class CommandEventPayloadContext : JsonSerializerContext { }
