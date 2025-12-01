using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Logging;

namespace NotificationService.Infrastructure.Repositories;

public class MongoLogRepository(IMongoContext context) : ILogRepository
{
    private readonly IMongoCollection<NotificationLogEntry> _mongoCollection =
        context.Database.GetCollection<NotificationLogEntry>(nameof(NotificationLogEntry));

    public async Task AddLogEntry(NotificationLogEntry notificationLogEntry, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(notificationLogEntry, new InsertOneOptions(), cancellationToken);
    }
}
