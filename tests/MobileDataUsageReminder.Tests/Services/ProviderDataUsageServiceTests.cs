using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

public class ProviderDataUsageServiceTests
{
    private readonly IApplicationConfiguration _mockApplicationConfiguration;
    private readonly IDataProviderGateway _mockProviderGateway;
    private readonly IMapperService _mockMapperService;
    private readonly ITelegramApiConfiguration _mockTelegramApiConfiguration;
    private readonly ProviderDataUsageService _providerDataUsageService;

    private const string _mockPhoneNumber = "123";

    public ProviderDataUsageServiceTests()
    {
        _mockApplicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _mockProviderGateway = Substitute.For<IDataProviderGateway>();
        _mockMapperService = Substitute.For<IMapperService>();

        var telegramUsers = new List<TelegramUser>
        {
            new TelegramUser { PhoneNumber = _mockPhoneNumber, ChatId = "1" }
        };

        _mockTelegramApiConfiguration = new TelegramApiConfiguration { TelegramUsers = telegramUsers };

        _providerDataUsageService = new ProviderDataUsageService(
            _mockApplicationConfiguration,
            _mockProviderGateway,
            _mockTelegramApiConfiguration,
            _mockMapperService);
    }

    [Fact]
    public void EmptyDataUsage_ShouldNot_MapToMobileData()
    {
        _mockProviderGateway.GetDataUsages(Arg.Any<ProviderCredentials>(), Arg.Any<List<TelegramUser>>())
                    .Returns(Task.FromResult(new List<DataUsage>()));

        _mockMapperService.DidNotReceive().MapMobileData(Arg.Any<DataUsage>());
    }

    [Fact]
    public async Task WithQuantityOfDataUsage_Should_MapToMobileData()
    {
        var dataUsages = new List<DataUsage>
        {
            new DataUsage { InitialAmount = 1 },
            new DataUsage { InitialAmount = 2 },
            new DataUsage { InitialAmount = 3 }
        };

        _mockProviderGateway.GetDataUsages(Arg.Any<ProviderCredentials>(), Arg.Any<List<TelegramUser>>())
                    .Returns(Task.FromResult(dataUsages));

        await _providerDataUsageService.GetMobileData();

        _mockMapperService.Received(dataUsages.Count).MapMobileData(Arg.Any<DataUsage>());
    }
}
