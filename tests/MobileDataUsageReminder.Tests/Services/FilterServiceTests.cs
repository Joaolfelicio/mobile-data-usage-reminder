using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

[TestClass]
public class FilterServiceTests
{
    private readonly FilterService _filterService;
    private readonly IMobileDataRepository _mockRepository;
    private readonly ILogger<FilterService> _mockLogger;

    public FilterServiceTests()
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
            new MobileData { UsedPercentage = 10 },
            new MobileData { UsedPercentage = 20 },
            new MobileData { UsedPercentage = 90 }
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
            new MobileData { UsedPercentage = 0 },
            new MobileData { UsedPercentage = -10 },
            new MobileData { UsedPercentage = -100 }
        };

        var filteredMobileDatas = _filterService.FilterNewMobileDatas(mobileDatas);

        filteredMobileDatas.Should().BeEmpty();
    }

    [TestMethod]
    public void Filtering_MobileData_ShouldReturn_OnlyTheOnesThatWereNotSentYet()
    {
        var sentMobileDataGuid = Guid.NewGuid();
        var notSentMobileDataGuid = Guid.NewGuid();

        _mockRepository.WasReminderAlreadySent(Arg.Any<MobileData>()).Returns(false);
        _mockRepository.WasReminderAlreadySent(Arg.Is<MobileData>(x => x.Id == sentMobileDataGuid)).Returns(true);

        var mobileDatas = new List<MobileData>()
        {
            new MobileData { Id = sentMobileDataGuid, UsedPercentage = 50 },
            new MobileData { Id = notSentMobileDataGuid, UsedPercentage = 60 },
        };

        var filteredMobileDatas = _filterService.FilterNewMobileDatas(mobileDatas);

        filteredMobileDatas.Should().HaveCount(1);
        filteredMobileDatas.Should().Contain(x => x.Id == notSentMobileDataGuid);
    }
}
