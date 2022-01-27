using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Constants.Contracts.Orange;
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
        private readonly HttpClient _httpClient;

        public OrangeGateway(
            IOrangeEndpoints orangeEndpoints,
            IOrangeConstants orangeConstants,
            ILogger<OrangeGateway> logger,
            HttpClient httpClient)
        {
            _orangeEndpoints = orangeEndpoints;
            _orangeConstants = orangeConstants;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<DataUsage>> GetDataUsages(ProviderCredentials providerCredentials, List<TelegramUser> telegramUsers)
        {
            await Login(providerCredentials);

            var clientId = await GetClientId();

            var dataProducts = await GetMobileDataProducts(telegramUsers, clientId);

            var dataUsages = new List<DataUsage>();
            foreach (var dataProduct in dataProducts)
            {
                var dataUsage = await GetDataUsage(dataProduct, clientId);
                dataUsages.Add(dataUsage);
            }

            return dataUsages;
        }

        private async Task Login(ProviderCredentials providerCredentials)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            var data = ConvertToJsonData(providerCredentials);

            var response = await _httpClient.PostAsync(_orangeEndpoints.LoginEndpoint, data);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<LoginResult>(responseString);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(responseData.TokenType, responseData.TokenValue);
        }

        private async Task<string> GetClientId()
        {
            var response = await _httpClient.GetAsync(_orangeEndpoints.ClientEndpoint);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ClientResult>(responseString);

            return responseData.PartyRole.Id;
        }

        private async Task<List<DataProduct>> GetMobileDataProducts(List<TelegramUser> telegramUsers, string clientId)
        {
            var response = await _httpClient.GetAsync(_orangeEndpoints.ProductEndpoint(clientId));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<ProductsResult>(responseString);

            List<DataProduct> dataProducts = ProjectDataProducts(telegramUsers, responseData);

            _logger.LogInformation($"Found {dataProducts.Count} products in orange.");
            return dataProducts;
        }

        private async Task<DataUsage> GetDataUsage(DataProduct dataProduct, string clientId)
        {
            var response = await _httpClient.GetAsync(_orangeEndpoints.DataConsumptionEndpoint(clientId, dataProduct.Id));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<DataConsumptionResult>(responseString);

            var dataConsumption = responseData.DataConsumptions.Find(x => x.Name == _orangeConstants.DataTypeName);

            _ = dataConsumption ?? throw new Exception($"Failed to get the data usage for {dataProduct.TelegramUser.PhoneNumber} in orange: {response.ReasonPhrase}");

            return new DataUsage
            {
                Unit = dataConsumption.Amount.Unit,
                InitialAmount = dataConsumption.Amount.InitialAmount,
                UsedAmount = dataConsumption.Amount.UsedAmount,
                RemainingAmount = dataConsumption.Amount.RemainingAmount,
                TelegramUser = dataProduct.TelegramUser
            };
        }

        private List<DataProduct> ProjectDataProducts(List<TelegramUser> telegramUsers, ProductsResult responseData)
        {
            // Get the products that have the same value for the phone number as passed in the argument
            var mobileDataProducts = responseData.Products
                .Where(x => x.PackageId == _orangeConstants.PackageId && x.Descriptions
                .Any(y => y.Name == "Phone number" && telegramUsers.Any(x => x.PhoneNumber == y.CurrentValue.Value)));

            var dataProducts = new List<DataProduct>();
            foreach (var responseProduct in mobileDataProducts)
            {
                var phoneNumber = responseProduct.Descriptions
                    .Find(x => x.Name == "Phone number" &&
                        telegramUsers.Any(y => y.PhoneNumber == x.CurrentValue.Value))?.CurrentValue.Value;

                var chatId = telegramUsers.Find(x => x.PhoneNumber == phoneNumber)?.ChatId;

                var product = new DataProduct
                {
                    Id = responseProduct.Id,
                    PackageId = responseProduct.PackageId,
                    TelegramUser = new TelegramUser { PhoneNumber = phoneNumber, ChatId = chatId }
                };
                dataProducts.Add(product);
            }

            return dataProducts;
        }

        private StringContent ConvertToJsonData<T>(T request) =>
            new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
    }
}