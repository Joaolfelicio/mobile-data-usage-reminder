using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Infrastructure.Models;
using MobileDataUsageReminder.Models;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure
{
    public class TelegramGateway : IReminderGateway
    {
        private readonly ITelegramApiConfiguration _telegramApiConfiguration;
        private readonly ILogger<ITelegramApiConfiguration> _logger;

        public TelegramGateway(ITelegramApiConfiguration telegramApiConfiguration,
            ILogger<ITelegramApiConfiguration> logger)
        {
            _telegramApiConfiguration = telegramApiConfiguration;
            _logger = logger;
        }

        /// <summary>
        /// Sends the post to API reminder.
        /// </summary>
        /// <param name="mobileDataPackage">The mobile data package.</param>
        public async Task SendPostToApiReminder(MobileDataPackage mobileDataPackage)
        {
            var reminder = new TelegramReminder
            {
                ChatId = mobileDataPackage.ChatId,
                ParseMode = "HTML",
                Text = $"Mobile Data Usage Reminder: Your mobile data plan has reached " +
                       $"<strong>{mobileDataPackage.UsedPercentage}%</strong> of the total of <em>{mobileDataPackage.InitialAmount}{mobileDataPackage.Unit}</em> " +
                       $"that you have for the month of {mobileDataPackage.Month}."
            };

            var data = ConvertToJsonData(reminder);

            var urlTelegramMessage = _telegramApiConfiguration.ApiEndPoint + _telegramApiConfiguration.AccessToken + "/sendMessage";

            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync(urlTelegramMessage, data);

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully sent reminder to {mobileDataPackage.PhoneNumber}");
                }
                else
                {
                    _logger.LogInformation($"Failed to send reminder to {mobileDataPackage.PhoneNumber}, reason: {result.ReasonPhrase}");
                }
            }
        }


        /// <summary>Converts to json data.</summary>
        /// <param name="reminder"></param>
        /// <returns>The Json Data.</returns>
        private StringContent ConvertToJsonData(Reminder reminder)
        {
            var messageJson = JsonConvert.SerializeObject(reminder);

            var data = new StringContent(messageJson, Encoding.UTF8, "application/json");

            return data;
        }
    }
}