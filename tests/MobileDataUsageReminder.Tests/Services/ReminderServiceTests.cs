using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestClass]
public class ReminderServiceTests
{
    private INotificationGateway _mockNotificationGateway;
    private ReminderService _reminderService;

    public ReminderServiceTests()
    {
        _mockNotificationGateway = Substitute.For<INotificationGateway>();
        _reminderService = new ReminderService(_mockNotificationGateway);
    }

    [TestMethod]
    public async Task Sending_Reminders_Should_CallTheGatewayForEachReminderToBeSent()
    {
        var mobileDatas = new List<MobileData>()
        {
            new MobileData { UsedPercentage = 10 },
            new MobileData { UsedPercentage = 20 },
            new MobileData { UsedPercentage = 90 }
        };

        await _reminderService.SendReminder(mobileDatas);

        await _mockNotificationGateway.Received(mobileDatas.Count).SendNotification(Arg.Any<MobileData>());
    }

    [TestMethod]
    public async Task Sending_Reminders_Shouldnt_CallTheGatewayIfItIsEmpty()
    {
        var mobileDatas = new List<MobileData>();

        await _reminderService.SendReminder(mobileDatas);

        await _mockNotificationGateway.Received(mobileDatas.Count).SendNotification(Arg.Any<MobileData>());
    }
}
