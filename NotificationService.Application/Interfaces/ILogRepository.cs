using NotificationService.Domain.Logging;

namespace NotificationService.Application.Interfaces;

public interface ILogRepository
{
    Task AddLogEntry(NotificationLogEntry entry, CancellationToken cancellationToken);
    Task<List<NotificationLogEntry>> GetFailedLogs(DateTime since, CancellationToken cancellationToken);
    Task<NotificationLogEntry?> GetByEventId(Guid eventId, CancellationToken cancellationToken);
}
