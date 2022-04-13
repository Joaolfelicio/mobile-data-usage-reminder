using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

public class MapperServiceTests
{
    private readonly MapperService _mapperService;

    public MapperServiceTests()
    {
        _mapperService = new MapperService();
    }

    [Theory]
    [InlineData(50, 50, 50)]
    [InlineData(50.9, 50.1, 50)]
    [InlineData(30, 70, 70)]
    [InlineData(35, 65, 70)]
    [InlineData(40, 60, 60)]
    [InlineData(5, 95, 100)]
    [InlineData(95, 5, 10)]
    [InlineData(94.9, 5.1, 10)]
    public void Map_DataUsageToMobileData_ShouldHaveCorrectUsagedPercentage(float remainingAmount, float usedAmount, int expectedPercentage)
    {
        var dataUsage = new DataUsage { RemainingAmount = remainingAmount, UsedAmount = usedAmount, InitialAmount = 100 };

        var result = _mapperService.MapMobileDataRoundUpPercent(new List<DataUsage> { dataUsage });

        result.First().UsedPercentage.Should().Be(expectedPercentage);
    }
}
