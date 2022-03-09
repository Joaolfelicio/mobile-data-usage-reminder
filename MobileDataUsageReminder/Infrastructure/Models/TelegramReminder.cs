using MobileDataUsageReminder.Models;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class TelegramReminder : Reminder
    {
        /// <summary>
        /// Gets or sets the chat identifier.
        /// </summary>
        /// <value>The chat identifier.</value>
        [JsonPropertyName("chat_id")]
        public string ChatId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the parse mode for the format.
        /// </summary>
        /// <value>The parse mode.</value>
        [JsonPropertyName("parse_mode")]
        public string ParseMode { get; set; }
    }
}