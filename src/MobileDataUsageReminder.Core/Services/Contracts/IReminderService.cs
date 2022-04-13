public interface IReminderService
{
    Task SendReminders(IList<MobileData> dataUsages);
}
