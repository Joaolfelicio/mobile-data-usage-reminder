
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class OrangeGateway : IDataProviderGateway
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
        if (_httpClient.DefaultRequestHeaders.Any())
        {
            _httpClient.DefaultRequestHeaders.Clear();
        }


        var response = await _httpClient.PostAsJsonAsync(_orangeEndpoints.LoginEndpoint, providerCredentials);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize(responseString, LoginResultContext.Default.LoginResult);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(responseData.TokenType, responseData.TokenValue);
    }

    private async Task<string> GetClientId()
    {
        var response = await _httpClient.GetFromJsonAsync(_orangeEndpoints.ClientEndpoint, ClientResultContext.Default.ClientResult);

        return response.PartyRole.Id;
    }

    private async Task<List<DataProduct>> GetMobileDataProducts(List<TelegramUser> telegramUsers, string clientId)
    {
        var response = await _httpClient.GetFromJsonAsync(_orangeEndpoints.ProductEndpoint(clientId), ProductsResultContext.Default.ProductsResult);

        List<DataProduct> dataProducts = ProjectDataProducts(telegramUsers, response);

        _logger.LogInformation("Found {count} products in orange.", dataProducts.Count);
        return dataProducts;
    }

    private async Task<DataUsage> GetDataUsage(DataProduct dataProduct, string clientId)
    {
        var response = await _httpClient.GetFromJsonAsync(
            _orangeEndpoints.DataConsumptionEndpoint(clientId, dataProduct.Id),
            DataConsumptionResultContext.Default.DataConsumptionResult);

        var dataConsumption = response.DataConsumptions.Find(x => x.Name == _orangeConstants.DataTypeName);

        _ = dataConsumption ?? throw new Exception($"Failed to get the data usage for {dataProduct.TelegramUser.PhoneNumber} in orange.");

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
}