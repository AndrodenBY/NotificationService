using MongoDB.Driver;
using NotificationService.Domain.Logging;

namespace NotificationService.Infrastructure.Helpers;

public static class MongoCollectionInitializer
{
    public static IMongoCollection<NotificationLogEntry> InitializeCollection(
        string connectionString,
        string databaseName,
        string collectionName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<NotificationLogEntry>(collectionName);

        var indexKeys = Builders<NotificationLogEntry>.IndexKeys.Ascending(e => e.EventId);
        collection.Indexes.CreateOne(new CreateIndexModel<NotificationLogEntry>(indexKeys));

        return collection;
    }
}
