using System.Text.Json.Serialization;

internal record TelegramReminder([property:JsonPropertyName("chat_id")] string ChatId, [property:JsonPropertyName("text")] string Text, [property:JsonPropertyName("parse_mode")] string ParseMode) : Reminder;

[JsonSerializable(typeof(TelegramReminder))]
internal partial class TelegramReminderContext : JsonSerializerContext {}