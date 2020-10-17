using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure
{
    public class OrangeGateway : IProviderGateway
    {
        private string TokenValue { get; set; }
        private string ClientId { get; set; }

        public LoginResult Login(string username, string password)
        {
            
            
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsync(urlTelegramMessage, data);
            }
        }

        public string GetClientId()
        {
            throw new System.NotImplementedException();
        }

        public List<DataProduct> GetDataProducts()
        {
            throw new System.NotImplementedException();
        }

        public DataUsage GetDataUsage(DataProduct dataProduct)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Converts to json data.
        /// </summary>
        /// <returns>The Json Data.</returns>
        private StringContent ConvertToJsonData<T>(T request)
        {
            var requestJson = JsonConvert.SerializeObject(request);

            var data = new StringContent(requestJson, Encoding.UTF8, "application/json");

            return data;
        }
    }
}