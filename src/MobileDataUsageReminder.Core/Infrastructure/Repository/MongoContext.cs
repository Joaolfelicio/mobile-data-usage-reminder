using MongoDB.Driver;

public class MongoContext : IMongoContext
{
    public MongoContext(MongoConfiguration config)
    {
        Collection = InitDatabase(config).GetCollection<MobileData>(config.CollectionName);

        SetupDatabase();
    }

    private void SetupDatabase()
    {
        var indexKeysDefinition = Builders<MobileData>.IndexKeys
        .Ascending(md => md.Month)
        .Ascending(md => md.PhoneNumber);

        Collection.Indexes.CreateOne(new CreateIndexModel<MobileData>(indexKeysDefinition));
    }

    private IMongoDatabase InitDatabase(MongoConfiguration config)
    {
        var connectionString = MongoClientSettings.FromConnectionString(config.ConnectionString);

        return new MongoClient(connectionString).GetDatabase(config.DatabaseName);
    }

    public IMongoCollection<MobileData> Collection { get; private set; }

    public void Dispose() => GC.SuppressFinalize(this);
}