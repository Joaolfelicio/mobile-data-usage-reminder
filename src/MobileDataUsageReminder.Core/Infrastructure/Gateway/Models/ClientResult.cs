
using System.Text.Json.Serialization;

internal record ClientResult([property:JsonPropertyName("partyRole")] PartyRole PartyRole);

internal record PartyRole([property:JsonPropertyName("id")] string Id);

[JsonSerializable(typeof(ClientResult))]
internal partial class ClientResultContext : JsonSerializerContext {}
