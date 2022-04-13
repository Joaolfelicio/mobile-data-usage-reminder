using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class MobileDataUsageFunctionTests
{
    private readonly IProviderDataUsageService _mockProviderDataUsage;
    private readonly IMapperService _mockMapperService;
    private readonly IReminderService _mockReminderService;
    private readonly ILogger<MobileDataUsageReminderFunction> _mockLogger;
    private readonly IMobileDataRepository _mockMobileDataRepository;
    private readonly IFilterService _mockFilterService;
    private readonly MobileDataUsageReminderFunction _mobileDataUsageReminder;

    public MobileDataUsageFunctionTests()
    {
        _mockProviderDataUsage = Substitute.For<IProviderDataUsageService>();
        _mockMapperService = Substitute.For<IMapperService>();
        _mockReminderService = Substitute.For<IReminderService>();
        _mockLogger = Substitute.For<ILogger<MobileDataUsageReminderFunction>>();
        _mockMobileDataRepository = Substitute.For<IMobileDataRepository>();
        _mockFilterService = Substitute.For<IFilterService>();

        _mobileDataUsageReminder = new MobileDataUsageReminderFunction(
            _mockProviderDataUsage,
            _mockMapperService,
            _mockReminderService,
            _mockLogger,
            _mockMobileDataRepository,
            _mockFilterService);
    }

    [Fact]
    public async Task SendingAndStoring_Reminders_ShouldBeCalled_WhenThereAreMobileDatas()
    {
        var mobileDatas = new List<MobileData>
        {
            new MobileData(),
            new MobileData()
        };
        _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

        await _mobileDataUsageReminder.Run(null);

        await _mockReminderService.Received(1).SendReminders(mobileDatas);
        await _mockMobileDataRepository.Received(1).CreateMobileData(mobileDatas);
    }

    [Fact]
    public async Task SendingAndStoring_Reminders_ShouldntBeCalled_IfThereAreNoMobileDatas()
    {
        var mobileDatas = new List<MobileData>();
        _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

        await _mobileDataUsageReminder.Run(null);

        await _mockReminderService.DidNotReceive().SendReminders(mobileDatas);
        await _mockMobileDataRepository.DidNotReceive().CreateMobileData(mobileDatas);
    }
}
