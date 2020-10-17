using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Constants.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Infrastructure.Models;
using MobileDataUsageReminder.Models;
using Newtonsoft.Json;

namespace MobileDataUsageReminder.Infrastructure
{
    public class OrangeGateway : IProviderGateway
    {
        private readonly IOrangeEndpoints _orangeEndpoints;
        private readonly ILogger<OrangeGateway> _logger;

        public OrangeGateway(IOrangeEndpoints orangeEndpoints,
            ILogger<OrangeGateway> logger)
        {
            _orangeEndpoints = orangeEndpoints;
            _logger = logger;
        }
        public string TokenValue { get; private set; }
        public string TokenType { get; private set; }
        public string ClientId { get; private set; }

        public async Task Login(string username, string password)
        {
            var loginRequest = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            var data = ConvertToJsonData(loginRequest);

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(_orangeEndpoints.LoginEndpoint, data);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully logged in into orange.");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<LoginResult>(responseString);

                    TokenType = responseData.TokenType;
                    TokenValue = responseData.TokenValue;
                }
                else
                {
                    throw new Exception($"Failed to login to orange: {response.ReasonPhrase}");
                }
            }
        }

        public async Task GetClientId()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<DataProduct>> GetDataProducts()
        {
            throw new System.NotImplementedException();
        }

        public async Task<DataUsage> GetDataUsage(DataProduct dataProduct)
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