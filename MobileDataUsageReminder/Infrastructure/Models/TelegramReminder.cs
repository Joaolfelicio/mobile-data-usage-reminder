using MobileDataUsageReminder.Models;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure.Models
{
    public class TelegramReminder : Reminder
    {
        /// <summary>
        /// Gets or sets the chat identifier.
        /// </summary>
        /// <value>The chat identifier.</value>
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the parse mode for the format.
        /// </summary>
        /// <value>The parse mode.</value>
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }
    }
}