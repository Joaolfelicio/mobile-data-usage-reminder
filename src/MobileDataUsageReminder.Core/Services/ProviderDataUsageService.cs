public class ProviderDataUsageService : IProviderDataUsageService
{
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IDataProviderGateway _dataProviderGateway;
    private readonly ITelegramApiConfiguration _telegramApiConfiguration;

    public ProviderDataUsageService(
        IApplicationConfiguration applicationConfiguration,
        IDataProviderGateway dataProviderGateway,
        ITelegramApiConfiguration telegramApiConfiguration)
    {
        _applicationConfiguration = applicationConfiguration;
        _dataProviderGateway = dataProviderGateway;
        _telegramApiConfiguration = telegramApiConfiguration;
    }

    public async Task<IEnumerable<DataUsage>> GetDataUsage()
    {
        var providerCred = new ProviderCredentials(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

        return await _dataProviderGateway.GetDataUsages(providerCred, _telegramApiConfiguration.TelegramUsers);
    }
}
