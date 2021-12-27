using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.Components;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.DAL.Repository.Contracts;
using MobileDataUsageReminder.Services.Contracts;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileDataUsageReminder.Tests.Components
{
    [TestClass]
    public class MobileDataUsageProcessorTests
    {
        private IProviderDataUsage _mockProviderDataUsage;
        private IReminderService _mockReminderService;
        private ILogger<MobileDataUsageProcessor> _mockLogger;
        private IMobileDataRepository _mockMobileDataRepository;
        private IFilterService _mockFilterService;
        private MobileDataUsageProcessor _mobileDataUsageProcessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockProviderDataUsage = Substitute.For<IProviderDataUsage>();
            _mockReminderService = Substitute.For<IReminderService>();
            _mockLogger = Substitute.For<ILogger<MobileDataUsageProcessor>>();
            _mockMobileDataRepository = Substitute.For<IMobileDataRepository>();
            _mockFilterService = Substitute.For<IFilterService>();

            _mobileDataUsageProcessor = new MobileDataUsageProcessor(_mockProviderDataUsage, _mockReminderService, 
                                                                     _mockLogger, _mockMobileDataRepository, _mockFilterService);
        }

        [TestMethod]
        public async Task SendingAndStoring_Reminders_ShouldBeCalled_WhenThereAreMobileDatas()
        {
            var mobileDatas = new List<MobileData>
            {
                new MobileData { Id = 1 },
                new MobileData { Id = 2 }
            };
            _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

            await _mobileDataUsageProcessor.ProcessMobileDataUsage();

            await _mockReminderService.Received(1).SendReminder(mobileDatas);
            await _mockMobileDataRepository.Received(1).CreateMobileDatas(mobileDatas);
        }

        [TestMethod]
        public async Task SendingAndStoring_Reminders_ShouldntBeCalled_IfThereAreNoMobileDatas()
        {
            var mobileDatas = new List<MobileData>();
            _mockFilterService.FilterNewMobileDatas(Arg.Any<List<MobileData>>()).Returns(mobileDatas);

            await _mobileDataUsageProcessor.ProcessMobileDataUsage();

            await _mockReminderService.DidNotReceive().SendReminder(mobileDatas);
            await _mockMobileDataRepository.DidNotReceive().CreateMobileDatas(mobileDatas);
        }
    }
}
