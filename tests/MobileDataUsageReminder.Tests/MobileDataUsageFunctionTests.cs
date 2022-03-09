using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace MobileDataUsageReminder.Tests.Components
{
    [TestClass]
    public class MobileDataUsageFunctionTests
    {
        private IProviderDataUsageService _mockProviderDataUsage;
        private IReminderService _mockReminderService;
        private ILogger<MobileDataUsageReminderFunction> _mockLogger;
        private IMobileDataRepository _mockMobileDataRepository;
        private IFilterService _mockFilterService;
        private MobileDataUsageReminderFunction _mobileDataUsageReminder;

        public MobileDataUsageFunctionTests()
        {
            _mockProviderDataUsage = Substitute.For<IProviderDataUsageService>();
            _mockReminderService = Substitute.For<IReminderService>();
            _mockLogger = Substitute.For<ILogger<MobileDataUsageReminderFunction>>();
            _mockMobileDataRepository = Substitute.For<IMobileDataRepository>();
            _mockFilterService = Substitute.For<IFilterService>();

            _mobileDataUsageReminder = new MobileDataUsageReminderFunction(
                _mockProviderDataUsage,
                _mockReminderService,
                _mockLogger,
                _mockMobileDataRepository,
                _mockFilterService);
        }

        [TestMethod]
        public async Task SendingAndStoring_Reminders_ShouldBeCalled_WhenThereAreMobileDatas()
        {
            var mobileDatas = new List<MobileData>
            {
                new MobileData(),
                new MobileData()
            };
            _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

            await _mobileDataUsageReminder.Run(null);

            await _mockReminderService.Received(1).SendReminder(mobileDatas);
            await _mockMobileDataRepository.Received(1).CreateMobileData(mobileDatas);
        }

        [TestMethod]
        public async Task SendingAndStoring_Reminders_ShouldntBeCalled_IfThereAreNoMobileDatas()
        {
            var mobileDatas = new List<MobileData>();
            _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

            await _mobileDataUsageReminder.Run(null);

            await _mockReminderService.DidNotReceive().SendReminder(mobileDatas);
            await _mockMobileDataRepository.DidNotReceive().CreateMobileData(mobileDatas);
        }
    }
}
