using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Logging;

public class NotificationLogEntry
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid? GroupId { get; set; }
    public Guid? UserId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string TemplatePath { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public MemberRole? MemberRole { get; set; }
    public DateOnly? NewExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
}
