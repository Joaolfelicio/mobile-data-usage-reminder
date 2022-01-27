using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Services;
using NSubstitute;
using System.Collections.Generic;

namespace MobileDataUsageReminder.Tests.Services
{
    [TestClass]
    public class FilterServiceTests
    {
        private FilterService _filterService;
        private IMobileDataRepository _mockRepository;
        private ILogger<FilterService> _mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = Substitute.For<IMobileDataRepository>();
            _mockLogger = Substitute.For<ILogger<FilterService>>();

            _filterService = new FilterService(_mockRepository, _mockLogger);
        }

        [TestMethod]
        public void Filtering_MobileData_Should_ReturnThemIfTheReminderWasAlreadySent_And_TheUsedPercentageIsAboveZero()
        {
            _mockRepository.WasReminderAlreadySent(Arg.Any<MobileData>()).Returns(false);

            var mobileDatas = new List<MobileData>()
            {
                new MobileData { Id = 1, UsedPercentage = 10 },
                new MobileData { Id = 2, UsedPercentage = 20 },
                new MobileData { Id = 3, UsedPercentage = 90 }
            };

            var filteredMobileDatas = _filterService.FilterNewMobileDatas(mobileDatas);

            filteredMobileDatas.Should().HaveCount(mobileDatas.Count);
        }

        [TestMethod]
        public void Filtering_MobileData_Shouldnt_ReturnThemIfTheUsedPercentageIsBelowOrEqualToZero()
        {
            _mockRepository.WasReminderAlreadySent(Arg.Any<MobileData>()).Returns(false);

            var mobileDatas = new List<MobileData>()
            {
                new MobileData { Id = 1, UsedPercentage = 0 },
                new MobileData { Id = 2, UsedPercentage = -10 },
                new MobileData { Id = 3, UsedPercentage = -100 }
            };

            var filteredMobileDatas = _filterService.FilterNewMobileDatas(mobileDatas);

            filteredMobileDatas.Should().BeEmpty();
        }

        [TestMethod]
        public void Filtering_MobileData_ShouldReturn_OnlyTheOnesThatWereNotSentYet()
        {
            const int sentMobileDataId = 1;
            const int notSentMobileDataId = 2;

            _mockRepository.WasReminderAlreadySent(Arg.Any<MobileData>()).Returns(false);
            _mockRepository.WasReminderAlreadySent(Arg.Is<MobileData>(x => x.Id == sentMobileDataId)).Returns(true);

            var mobileDatas = new List<MobileData>()
            {
                new MobileData { Id = sentMobileDataId, UsedPercentage = 50 },
                new MobileData { Id = notSentMobileDataId, UsedPercentage = 60 },
            };

            var filteredMobileDatas = _filterService.FilterNewMobileDatas(mobileDatas);

            filteredMobileDatas.Should().HaveCount(1);
            filteredMobileDatas.Should().Contain(x => x.Id == notSentMobileDataId);
        }
    }
}
