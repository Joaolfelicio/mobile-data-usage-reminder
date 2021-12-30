using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services;
using MobileDataUsageReminder.Services.Contracts;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileDataUsageReminder.Tests.Services
{
    [TestClass]
    public class ProviderDataUsageServiceTests
    {
        private IApplicationConfiguration _mockApplicationConfiguration;
        private IProviderGateway _mockProviderGateway;
        private IMapperService _mockMapperService;
        private ITelegramApiConfiguration _mockTelegramApiConfiguration;
        private ProviderDataUsageService _providerDataUsageService;

        private const string _mockPhoneNumber = "123";

        [TestInitialize]
        public void TestInitialize()
        {
            _mockApplicationConfiguration = Substitute.For<IApplicationConfiguration>();
            _mockProviderGateway = Substitute.For<IProviderGateway>();
            _mockMapperService = Substitute.For<IMapperService>();

            var telegramUsers = new List<TelegramUser>
            {
                new TelegramUser { PhoneNumber = _mockPhoneNumber, ChatId = "1" }
            };

            _mockTelegramApiConfiguration = new TelegramApiConfiguration { TelegramUsers = telegramUsers };

            _providerDataUsageService = new ProviderDataUsageService(_mockApplicationConfiguration, _mockProviderGateway, _mockTelegramApiConfiguration, _mockMapperService);
        }

        [TestMethod]
        public void EmptyDataUsage_ShouldNot_MapToMobileData()
        {
            _mockProviderGateway.GetDataUsages(Arg.Any<ProviderCredentials>(), Arg.Any<List<TelegramUser>>())
                        .Returns(Task.FromResult(new List<DataUsage>()));

            _mockMapperService.DidNotReceive().MapMobileData(Arg.Any<DataUsage>());
        }

        [TestMethod]
        public async Task WithQuantityOfDataUsage_Should_MapToMobileData()
        {
            var dataUsages = new List<DataUsage>
            {
                new DataUsage { InitialAmount = "1"},
                new DataUsage { InitialAmount = "2"},
                new DataUsage { InitialAmount = "3"}
            };

            _mockProviderGateway.GetDataUsages(Arg.Any<ProviderCredentials>(), Arg.Any<List<TelegramUser>>())
                        .Returns(Task.FromResult(dataUsages));

            await _providerDataUsageService.GetMobileData();

            _mockMapperService.Received(dataUsages.Count).MapMobileData(Arg.Any<DataUsage>());
        }
    }
}
