namespace NotificationService.Interfaces;

public interface IBaseEvent
{
    DateTime SendedAt { get; init; }
}