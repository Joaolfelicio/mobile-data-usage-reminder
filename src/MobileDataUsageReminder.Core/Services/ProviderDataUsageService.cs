public class ProviderDataUsageService : IProviderDataUsageService
{
    private readonly IApplicationConfiguration _applicationConfiguration;
    private readonly IDataProviderGateway _dataProviderGateway;
    private readonly ITelegramApiConfiguration _telegramApiConfiguration;
    private readonly IMapperService _mapperService;

    public ProviderDataUsageService(
        IApplicationConfiguration applicationConfiguration,
        IDataProviderGateway dataProviderGateway,
        ITelegramApiConfiguration telegramApiConfiguration,
        IMapperService mapperService)
    {
        _applicationConfiguration = applicationConfiguration;
        _dataProviderGateway = dataProviderGateway;
        _telegramApiConfiguration = telegramApiConfiguration;
        _mapperService = mapperService;
    }

    public async Task<List<MobileData>> GetMobileData()
    {
        var providerCred = new ProviderCredentials(_applicationConfiguration.ProviderEmail, _applicationConfiguration.ProviderPassword);

        var dataUsages = await _dataProviderGateway.GetDataUsages(providerCred, _telegramApiConfiguration.TelegramUsers);

        var mobileDatas = new List<MobileData>();
        foreach (var dataUsage in dataUsages)
        {
            mobileDatas.Add(_mapperService.MapMobileData(dataUsage));
        }

        return mobileDatas;
    }
}
