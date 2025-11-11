using NotificationService.Domain.Logging;

namespace NotificationService.Application.Interfaces;

public interface ILogRepository
{
    Task AddLogEntry(NotificationLogEntry entry, CancellationToken cancellationToken);
}
