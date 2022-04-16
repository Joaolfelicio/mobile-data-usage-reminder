public class ReminderService : IReminderService
{
    private readonly INotificationGateway _notificationGateway;

    public ReminderService(INotificationGateway notificationGateway)
    {
        _notificationGateway = notificationGateway;
    }

    public async Task SendReminders(IList<MobileData> dataUsages)
    {
        var notificationTasks = new Task[dataUsages.Count];

        for (var i = 0; i < dataUsages.Count; i++)
        {
            notificationTasks[i] = _notificationGateway.SendNotification(dataUsages[i]);
        }

        await Task.WhenAll(notificationTasks);
    }
}
