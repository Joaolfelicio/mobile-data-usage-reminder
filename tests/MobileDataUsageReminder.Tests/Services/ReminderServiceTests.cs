using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class ReminderServiceTests
{
    private INotificationGateway _mockNotificationGateway;
    private ReminderService _reminderService;

    public ReminderServiceTests()
    {
        _mockNotificationGateway = Substitute.For<INotificationGateway>();
        _reminderService = new ReminderService(_mockNotificationGateway);
    }

    [Fact]
    public async Task Sending_Reminders_Should_CallTheGatewayForEachReminderToBeSent()
    {
        var mobileDatas = new List<MobileData>()
        {
            new MobileData { UsedPercentage = 10 },
            new MobileData { UsedPercentage = 20 },
            new MobileData { UsedPercentage = 90 }
        };

        await _reminderService.SendReminders(mobileDatas);

        await _mockNotificationGateway.Received(mobileDatas.Count).SendNotification(Arg.Any<MobileData>());
    }

    [Fact]
    public async Task Sending_Reminders_Shouldnt_CallTheGatewayIfItIsEmpty()
    {
        var mobileDatas = new List<MobileData>();

        await _reminderService.SendReminders(mobileDatas);

        await _mockNotificationGateway.Received(mobileDatas.Count).SendNotification(Arg.Any<MobileData>());
    }
}
