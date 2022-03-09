using System.Text.Json.Serialization;

public class TelegramReminder : Reminder
{
    [JsonPropertyName("chat_id")]
    public string ChatId { get; init; }

    [JsonPropertyName("text")]
    public string Text { get; init; }

    [JsonPropertyName("parse_mode")]
    public string ParseMode { get; init; }
}
