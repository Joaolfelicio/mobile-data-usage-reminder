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
        private readonly IApplicationConfiguration _applicationConfiguration;

        public TelegramGateway(IApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }
        //string UrlTelegramPhotoMessage = _telegramApiSettings.Url + Auth.AccessToken.ACCESS_TOKEN_TELEGRAM + "/sendMessage";

        public async Task SendPostToApiReminder(DataUsage dataUsage)
        {

            var reminder = new TelegramReminder
            {
                ChatId = 1,
                ParseMode = "HTML",
                Text = $"Mobile Data Usage Reminder: Your mobile data plan for <strong>{dataUsage.PhoneNumber}</strong> " +
                       $"has reached <strong>{dataUsage.DataUsedPercentage}%</strong> of the total of <em>{dataUsage.MonthlyDataGb}GB</em> " +
                       $"that you have for the month of {dataUsage.Month}."
            };

            var jsonData = ConvertToJsonData(reminder);

            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync("Test.com", jsonData);
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