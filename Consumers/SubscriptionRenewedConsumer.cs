using MassTransit;
using NotificationService.Constant;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Consumers;

public class SubscriptionRenewedConsumer(IEmailService emailService, ILogger<IEmailService> logger) : IConsumer<SubscriptionRenewedEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionRenewedEvent> context)
    {
        var subscriptionRenewedEvent = context.Message;

        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.SubscriptionRenewed, subscriptionRenewedEvent.Name),
            templatePath: TemplatePathConstants.SubscriptionRenewed,
            eventModel: subscriptionRenewedEvent,
            cancellationToken: context.CancellationToken
        );
    }
}
