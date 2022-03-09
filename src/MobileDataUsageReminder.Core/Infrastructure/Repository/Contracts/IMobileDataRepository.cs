public interface IMobileDataRepository
{
    bool WasReminderAlreadySent(MobileData mobileData);

    Task CreateMobileData(List<MobileData> mobileDatas);
}
