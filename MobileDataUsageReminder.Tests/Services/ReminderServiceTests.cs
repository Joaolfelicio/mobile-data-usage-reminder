using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileDataUsageReminder.DAL.Models;
using MobileDataUsageReminder.Infrastructure.Contracts;
using MobileDataUsageReminder.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileDataUsageReminder.Tests.Services
{
    [TestClass]
    public class ReminderServiceTests
    {
        private IReminderGateway _mockReminderGateway;
        private ReminderService _reminderService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockReminderGateway = Substitute.For<IReminderGateway>();
            _reminderService = new ReminderService(_mockReminderGateway);
        }

        [TestMethod]
        public async Task Sending_Reminders_Should_CallTheGatewayForEachReminderToBeSent()
        {
            var mobileDatas = new List<MobileData>()
            {
                new MobileData { Id = 1, UsedPercentage = 10 },
                new MobileData { Id = 2, UsedPercentage = 20 },
                new MobileData { Id = 3, UsedPercentage = 90 }
            };

            await _reminderService.SendReminder(mobileDatas);

            await _mockReminderGateway.Received(mobileDatas.Count).SendPostToApiReminder(Arg.Any<MobileData>());
        }

        [TestMethod]
        public async Task Sending_Reminders_Shouldnt_CallTheGatewayIfItIsEmpty()
        {
            var mobileDatas = new List<MobileData>();

            await _reminderService.SendReminder(mobileDatas);

            await _mockReminderGateway.Received(mobileDatas.Count).SendPostToApiReminder(Arg.Any<MobileData>());
        }
    }
}
