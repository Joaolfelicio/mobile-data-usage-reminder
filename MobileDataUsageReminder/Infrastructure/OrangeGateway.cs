using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly IOrangeConstants _orangeConstants;
        private readonly ILogger<OrangeGateway> _logger;

        public OrangeGateway(IOrangeEndpoints orangeEndpoints,
            IOrangeConstants orangeConstants,
            ILogger<OrangeGateway> logger)
        {
            _orangeEndpoints = orangeEndpoints;
            _orangeConstants = orangeConstants;
            _logger = logger;
        }
        public string TokenValue { get; private set; }
        public string TokenType { get; private set; }
        public string ClientId { get; private set; }


        /// <summary>
        /// Login to the provider, will store the TokenValue and the TokenType
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <exception cref="Exception">Failed to login to orange</exception>
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

        /// <summary>
        /// Get the client details and store the client id
        /// </summary>
        /// <exception cref="Exception">Failed to get the client in orange</exception>
        public async Task GetClient()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenType, TokenValue);

                var response = await httpClient.GetAsync(_orangeEndpoints.ClientEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully got the client in orange.");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ClientResult>(responseString);

                    ClientId = responseData.PartyRole.Id;
                }
                else
                {
                    throw new Exception($"Failed to get the client in orange: {response.ReasonPhrase}");
                }
            }
        }

        /// <summary>
        /// Get the mobile data products by filtering the ones that are related to the data mobile and the phone numbers
        /// </summary>
        /// <param name="productsPhoneNumber">The phone numbers to filter the products</param>
        /// <returns>The data products</returns>
        public async Task<List<DataProduct>> GetMobileDataProducts(List<string> productsPhoneNumber)
        {
            var dataProducts = new List<DataProduct>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenType, TokenValue);

                var response = await httpClient.GetAsync(_orangeEndpoints.ProductEndpoint(ClientId));

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully got the products in orange.");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ProductsResult>(responseString);

                    // Get the products that have the same value for the phone number as passed in the argument
                    var mobileDataProducts = responseData.Products
                        .Where(x => x.PackageId == _orangeConstants.PackageId && x.Descriptions
                            .Any(y => y.Name == "Phone number" &&
                                                            productsPhoneNumber.Contains(y.CurrentValue.Value)));

                    foreach (var responseProduct in mobileDataProducts)
                    {
                        var product = new DataProduct()
                        {
                            Id = responseProduct.Id,
                            PackageId = responseProduct.PackageId,
                        };

                        var phoneNumber = responseProduct.Descriptions
                            .FirstOrDefault(x => x.Name == "Phone number" &&
                                                 productsPhoneNumber.Contains(x.CurrentValue.Value))?.CurrentValue.Value;

                        product.PhoneNumber = phoneNumber;

                        dataProducts.Add(product);
                    }
                }
                else
                {
                    throw new Exception($"Failed to get the products in orange: {response.ReasonPhrase}");
                }

                _logger.LogInformation($"Found {dataProducts.Count} products in orange.");
                return dataProducts;
            }
        }

        /// <summary>
        /// Gets the data usage for the data product
        /// </summary>
        /// <param name="dataProduct"></param>
        /// <returns>
        /// The data usage
        /// </returns>
        /// <exception cref="Exception">Failed to get the data usage</exception>
        public async Task<DataUsage> GetDataUsage(DataProduct dataProduct)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TokenType, TokenValue);

                var response = await httpClient.GetAsync(_orangeEndpoints.DataConsumptionEndpoint(ClientId, dataProduct.Id));

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully got the data usage for {dataProduct.PhoneNumber} in orange.");

                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<DataConsumptionResult>(responseString);

                    foreach (var dataConsumption in responseData.DataConsumptions.Where(x => x.Name == _orangeConstants.DataTypeName))
                    {
                        return new DataUsage()
                        {
                            Unit = dataConsumption.Amount.Unit,
                            InitialAmount = dataConsumption.Amount.InitialAmount,
                            UsedAmount = dataConsumption.Amount.UsedAmount,
                            RemainingAmount = dataConsumption.Amount.RemainingAmount
                        };
                    }
                }

                throw new Exception($"Failed to get the data usage for {dataProduct.PhoneNumber} in orange: {response.ReasonPhrase}");
            }
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