using MongoDB.Driver;
using NotificationService.Domain.Logging;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Helpers;

public static class MongoCollectionInitializer
{
    public static IMongoCollection<NotificationLogEntry> InitializeCollection(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.MongoDbConnectionString);
        var database = client.GetDatabase(settings.MongoDbNotificationServiceDatabase);
        var collection = database.GetCollection<NotificationLogEntry>(settings.MongoDbNotificationServiceCollection);

        var indexKeys = Builders<NotificationLogEntry>.IndexKeys.Ascending(e => e.EventId);
        collection.Indexes.CreateOne(new CreateIndexModel<NotificationLogEntry>(indexKeys));

        return collection;
    }
}
