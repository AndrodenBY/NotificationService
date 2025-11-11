using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Logging;
using NotificationService.Infrastructure.Helpers;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Repositories;

public class MongoLogRepository(IOptions<MongoDbSettings> mongoSettings) : ILogRepository
{
    private readonly IMongoCollection<NotificationLogEntry> _mongoCollection = MongoCollectionInitializer.InitializeCollection(mongoSettings.Value);

    public async Task AddLogEntry(NotificationLogEntry notificationLogEntry, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(notificationLogEntry, new InsertOneOptions(), cancellationToken);
    }

    public async Task<List<NotificationLogEntry>> GetFailedLogs(DateTime since, CancellationToken cancellationToken)
    {
        return await _mongoCollection
            .Find(events => !events.Success && events.CreatedAt >= since)
            .SortByDescending(events => events.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<NotificationLogEntry?> GetByEventId(Guid eventId, CancellationToken cancellationToken)
    {
        return await _mongoCollection
            .Find(events => events.Id == eventId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
