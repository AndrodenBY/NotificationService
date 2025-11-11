using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Logging;

namespace NotificationService.Infrastructure.Repositories;

public class MongoLogRepository(IMongoCollection<NotificationLogEntry> mongoCollection) : ILogRepository
{
    public async Task AddLogEntry(NotificationLogEntry notificationLogEntry, CancellationToken cancellationToken)
    {
        await mongoCollection.InsertOneAsync(notificationLogEntry, new InsertOneOptions(), cancellationToken);
    }
}
