using System.Text.Json.Serialization;

public record CommandEventPayload([property: JsonPropertyName("update_id")] int Id, [property: JsonPropertyName("message")] Message Message);

public record Message([property: JsonPropertyName("message_id")] int Id, [property: JsonPropertyName("from")] User From, [property: JsonPropertyName("date")] int Date, [property: JsonPropertyName("text")] string CommandType);

public record User([property: JsonPropertyName("id")] int Id, [property: JsonPropertyName("username")] string Username);

[JsonSerializable(typeof(CommandEventPayload))]
public partial class CommandEventPayloadContext : JsonSerializerContext { }
