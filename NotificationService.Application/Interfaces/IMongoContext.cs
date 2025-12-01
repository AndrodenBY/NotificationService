using MongoDB.Driver;

namespace NotificationService.Application.Interfaces;

public interface IMongoContext
{
    IMongoDatabase Database { get; }
    IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class;
}