using MongoDB.Driver;
using NotificationService.Application.Interfaces;

namespace NotificationService.Infrastructure.Persistence;

public class MongoContext : IMongoContext
{
    public MongoContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        Database = client.GetDatabase(databaseName);
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<TDocument> GetCollection<TDocument>()
        where TDocument : class
    {
        return Database.GetCollection<TDocument>(typeof(TDocument).Name);
    }
}
