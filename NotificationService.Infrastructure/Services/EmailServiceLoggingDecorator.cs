using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Logging;
using NotificationService.Infrastructure.Helpers;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Services;

public class EmailServiceLoggingDecorator(IEmailService emailService, ILogRepository logRepository, ILogger<EmailServiceLoggingDecorator> logger) : IEmailService
{
    public async Task Send<TEvent>(
        string recipientEmail,
        string subject,
        string templatePath,
        TEvent eventModel,
        CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var log = new NotificationLogEntry
        {
            Id = Guid.NewGuid(),
            Recipient = recipientEmail,
            Subject = subject,
            TemplatePath = templatePath,
            EventType = typeof(TEvent).Name,
            CreatedAt = DateTime.UtcNow
        };
        
        switch (eventModel)
        {
            case MemberLeftGroupEvent notificationEvent:
                NotificationLogEntryFiller.FillMemberLeftLog(notificationEvent, log);
                break;

            case MemberChangedRoleEvent notificationEvent:
                NotificationLogEntryFiller.FillMemberChangedRoleLog(notificationEvent, log);
                break;

            case SubscriptionCanceledEvent notificationEvent:
                NotificationLogEntryFiller.FillSubscriptionCanceledLog(notificationEvent, log);
                break;

            case SubscriptionRenewedEvent notificationEvent:
                NotificationLogEntryFiller.FillSubscriptionRenewedLog(notificationEvent, log);
                break;

            default:
                logger.LogWarning("No log filler found for event type {EventType}", typeof(TEvent).Name);
                break;
        }

        try
        {
            await emailService.Send(recipientEmail, subject, templatePath, eventModel, cancellationToken);
            log.Success = true;
            logger.LogInformation("Email sent to {Recipient}", recipientEmail);
        }
        catch (Exception ex)
        {
            log.Success = false;
            log.Errors.Add(ex.Message);
            logger.LogError(ex, "Email failed to {Recipient}", recipientEmail);
        }
        finally
        {
            await logRepository.AddLogEntry(log, cancellationToken);
        }
    }
}
