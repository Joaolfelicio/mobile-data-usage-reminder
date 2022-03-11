public interface INotificationGateway
{
    Task SendNotification(MobileData mobileData);
}
