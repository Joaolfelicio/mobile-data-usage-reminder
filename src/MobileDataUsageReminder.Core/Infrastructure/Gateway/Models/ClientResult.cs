
using System.Text.Json.Serialization;

public class ClientResult
{
    [JsonPropertyName("partyRole")]
    public PartyRole PartyRole { get; init; }
}
