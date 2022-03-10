using MongoDB.Driver;

public interface IMongoContext : IDisposable
{
    IMongoCollection<MobileData> Collection {get; }
}