using NotificationService.Application.Mapping;
using NotificationService.Domain.Logging;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Helpers;

public static class NotificationLogEntryFiller
{
    private static void FillBaseLog(NotificationLogEntry log, Guid eventId, string recipient, string eventType)
    {
        log.EventId = eventId;
        log.Recipient = recipient;
        log.EventType = eventType;
    }
    
    public static void FillMemberLeftLog(MemberLeftGroupEvent notificationEvent, NotificationLogEntry log)
    {
        FillBaseLog(log, notificationEvent.Id, notificationEvent.Email, nameof(MemberLeftGroupEvent));
        log.GroupId = notificationEvent.GroupId;
    }

    public static void FillMemberChangedRoleLog(MemberChangedRoleEvent notificationEvent, NotificationLogEntry log)
    {
        FillBaseLog(log, notificationEvent.Id, notificationEvent.Email, nameof(MemberChangedRoleEvent));
        log.GroupId = notificationEvent.GroupId;
        log.MemberRole = EnumMapper.MapMemberRole(notificationEvent.Role);
    }

    public static void FillSubscriptionCanceledLog(SubscriptionCanceledEvent notificationEvent, NotificationLogEntry log)
    {
        FillBaseLog(log, notificationEvent.Id, notificationEvent.Email, nameof(SubscriptionCanceledEvent));
        log.UserId = notificationEvent.UserId;
    }

    public static void FillSubscriptionRenewedLog(SubscriptionRenewedEvent notificationEvent, NotificationLogEntry log)
    {
        FillBaseLog(log, notificationEvent.Id, notificationEvent.Email, nameof(SubscriptionRenewedEvent));
        log.UserId = notificationEvent.UserId;
        log.NewExpirationDate = notificationEvent.NewExpirationDate;
    }
}
