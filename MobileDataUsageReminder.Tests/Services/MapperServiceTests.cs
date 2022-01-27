using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.Models;
using MobileDataUsageReminder.Services;

namespace MobileDataUsageReminder.Tests.Services
{
    [TestClass]
    public class MapperServiceTests
    {
        private MapperService _mapperService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mapperService = new MapperService();
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
        public void Map_DataUsageToMobileData_ShouldHaveCorrectUsagedPercentage(string remainingAmount, string usedAmount, int expectedPercentage)
        {
            var dataUsage = new DataUsage { RemainingAmount = remainingAmount, UsedAmount = usedAmount, InitialAmount = "100" };

            var result = _mapperService.MapMobileData(dataUsage);

            result.UsedPercentage.Should().Be(expectedPercentage);
        }
    }
}
