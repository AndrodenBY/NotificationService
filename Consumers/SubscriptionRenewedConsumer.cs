using MassTransit;
using NotificationService.Constant;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Consumers;

public class SubscriptionRenewedConsumer(IEmailService emailService, ILogger<IEmailService> logger)
    : IConsumer<SubscriptionRenewedEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionRenewedEvent> context)
    {
        var subscriptionRenewedEvent = context.Message;

        await emailService.Send(
            subscriptionRenewedEvent.Email,
            string.Format(SubjectConstants.SubscriptionRenewed, subscriptionRenewedEvent.Name),
            TemplatePathConstants.SubscriptionRenewed,
            subscriptionRenewedEvent,
            context.CancellationToken
        );
    }
}
