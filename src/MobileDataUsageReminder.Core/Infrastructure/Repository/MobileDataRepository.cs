using MongoDB.Driver;

public class MobileDataRepository : IMobileDataRepository
{
    private readonly IMongoCollection<MobileData> _mongoCollection;

    public MobileDataRepository(IMongoContext mongoContext)
    {
        _mongoCollection = mongoContext.Collection;
    }

    public Task CreateMobileData(List<MobileData> mobileDatas) => _mongoCollection.InsertManyAsync(mobileDatas);


    public bool WasReminderAlreadySent(MobileData mobileData) =>
        _mongoCollection
            .AsQueryable()
            .Any(x => x.Month == DateTime.Now.ToString("MMMM") &&
                      x.Year == DateTime.Now.Year &&
                      x.PhoneNumber == mobileData.PhoneNumber &&
                      x.UsedPercentage == mobileData.UsedPercentage);
}
