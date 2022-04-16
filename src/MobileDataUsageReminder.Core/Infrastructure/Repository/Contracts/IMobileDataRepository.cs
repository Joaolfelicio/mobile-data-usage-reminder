public interface IMobileDataRepository
{
    bool WasReminderAlreadySent(MobileData mobileData);

    Task CreateMobileData(IEnumerable<MobileData> mobileDatas);
}
