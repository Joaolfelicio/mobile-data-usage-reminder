using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.Configurations;
using MobileDataUsageReminder.Configurations.Contracts;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileDataUsageReminder.Tests.Services
{
    [TestClass]
    public class OrangeDataUsageServiceTests
    {
        private IApplicationConfiguration _mockApplicationConfiguration;
        private ILogger<OrangeDataUsageService> _mockLogger;
        private IProviderGateway _mockProviderGateway;
        private ITelegramApiConfiguration _mockTelegramApiConfiguration;
        private OrangeDataUsageService _orangeDataUsageService;

        private const string _mockPhoneNumber = "123";

        [TestInitialize]
        public void TestInitialize()
        {
            _mockApplicationConfiguration = Substitute.For<IApplicationConfiguration>();
            _mockLogger = Substitute.For<ILogger<OrangeDataUsageService>>();
            _mockProviderGateway = Substitute.For<IProviderGateway>();

            var telegramUsers = new List<TelegramUser>
            {
                new TelegramUser { PhoneNumber = _mockPhoneNumber, ChatId = "1" }
            };
            _mockTelegramApiConfiguration = new TelegramApiConfiguration { TelegramUsers = telegramUsers };

            _orangeDataUsageService = new OrangeDataUsageService(_mockApplicationConfiguration, _mockLogger, 
                                                                 _mockProviderGateway, _mockTelegramApiConfiguration);
        }

        [TestMethod]
        public async Task Getting_ListOfMobileDatas_FromProvider_ShouldBeEmpty_IfNoProductsWereFound()
        {
            _mockProviderGateway.GetMobileDataProducts(Arg.Any<List<string>>()).Returns(new List<DataProduct>());

            var mobileDatas = await _orangeDataUsageService.GetMobileData();

            mobileDatas.Should().BeEmpty();
        }

        [TestMethod]
        public async Task Getting_ListOfMobileDatas_FromProvider_ShouldFetchTheDataUsage_ForThoseProducts()
        {
            var dataProducts = new List<DataProduct>()
            {
                new DataProduct { PhoneNumber = _mockPhoneNumber }
            };
            _mockProviderGateway.GetMobileDataProducts(Arg.Any<List<string>>()).Returns(dataProducts);
            
            var dataUsage = new DataUsage { InitialAmount = "10000", RemainingAmount = "5000", Unit = "MB", UsedAmount = "5000" };
            _mockProviderGateway.GetDataUsage(Arg.Any<DataProduct>()).Returns(Task.FromResult(dataUsage));

            await _orangeDataUsageService.GetMobileData();

            await _mockProviderGateway.Received(1).GetDataUsage(dataProducts.First());
        }

        [DataTestMethod]
        [DataRow("50", "50", 50)]
        [DataRow("50.9", "50.1", 50)]
        [DataRow("30", "70", 70)]
        [DataRow("35", "65", 70)]
        [DataRow("40", "60", 60)]
        [DataRow("5", "95", 100)]
        [DataRow("95", "5", 10)]
        [DataRow("94.9", "5.1", 10)]
        public async Task Projecting_DataUsageToMobileData_ShouldHaveCorrectUsagedPercentage(string remainingAmount, string usedAmount, int expectedPercentage)
        {
            var dataProducts = new List<DataProduct>()
            {
                new DataProduct { PhoneNumber = _mockPhoneNumber }
            };
            _mockProviderGateway.GetMobileDataProducts(Arg.Any<List<string>>()).Returns(dataProducts);

            var dataUsage = new DataUsage { InitialAmount = "100", RemainingAmount = remainingAmount, Unit = "MB", UsedAmount = usedAmount };
            _mockProviderGateway.GetDataUsage(Arg.Any<DataProduct>()).Returns(Task.FromResult(dataUsage));

            var mobileDatas = await _orangeDataUsageService.GetMobileData();

            mobileDatas.First().UsedPercentage.Should().Be(expectedPercentage);
        }
    }
}
