using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Infrastructure.Models;
using MobileDataUsageReminder.Models;
using System.Text.Json.Serialization;

namespace MobileDataUsageReminder.Infrastructure
{
    public class TelegramGateway : IReminderGateway
    {
        private readonly ITelegramApiConfiguration _telegramApiConfiguration;
        private readonly ILogger<ITelegramApiConfiguration> _logger;
        private readonly HttpClient _httpClient;

        public TelegramGateway(
            ITelegramApiConfiguration telegramApiConfiguration,
            ILogger<ITelegramApiConfiguration> logger,
            HttpClient httpClient)
        {
            _telegramApiConfiguration = telegramApiConfiguration;
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Sends the post to API reminder.
        /// </summary>
        /// <param name="mobileData">The mobile data package.</param>
        public async Task SendPostToApiReminder(MobileData mobileData)
        {
            var reminder = new TelegramReminder
            {
                ChatId = mobileData.ChatId,
                ParseMode = "HTML",
                Text =  "Mobile Data Usage Reminder: Your mobile data plan has reached " +
                       $"<strong>{mobileData.UsedPercentage}%</strong> of the total of <em>{mobileData.InitialAmount}{mobileData.Unit}</em> " +
                       $"that you have for the month of {mobileData.Month}."
            };

            var data = ConvertToJsonData(reminder);
            var urlTelegramMessage = _telegramApiConfiguration.ApiEndPoint + _telegramApiConfiguration.AccessToken + "/sendMessage";

            var result = await _httpClient.PostAsync(urlTelegramMessage, data);
            result.EnsureSuccessStatusCode();

            _logger.LogInformation($"Successfully sent reminder to {mobileData.PhoneNumber}");
        }

        /// <summary>Converts to json data.</summary>
        /// <param name="reminder"></param>
        /// <returns>The Json Data.</returns>
        private StringContent ConvertToJsonData(Reminder reminder) =>
            new StringContent(JsonConvert.SerializeObject(reminder), Encoding.UTF8, "application/json");
    }
}