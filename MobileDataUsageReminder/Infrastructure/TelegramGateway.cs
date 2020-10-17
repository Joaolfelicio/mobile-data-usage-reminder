using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure
{
    public class TelegramGateway : IReminderGateway
    {
        private readonly ITelegramApiConfiguration _telegramApiConfiguration;

        public TelegramGateway(ITelegramApiConfiguration telegramApiConfiguration)
        {
            _telegramApiConfiguration = telegramApiConfiguration;
        }

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
                await httpClient.PostAsync(urlTelegramMessage, data);
            }
        }


        /// <summary>
        /// Converts to json data.
        /// </summary>
        /// <param name="telegramMessage">The telegram message.</param>
        /// <returns>The Json Data.</returns>
        private StringContent ConvertToJsonData(Reminder reminder)
        {
            var messageJson = JsonConvert.SerializeObject(reminder);

            var data = new StringContent(messageJson, Encoding.UTF8, "application/json");

            return data;
        }
    }
}