using MassTransit;
using NotificationService.Constant;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Consumers;

public class SubscriptionCanceledConsumer(IEmailService emailService, ILogger<IEmailService> logger)
    : IConsumer<SubscriptionCanceledEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionCanceledEvent> context)
    {
        var subscriptionCanceledEvent = context.Message;

        await emailService.Send(
            subscriptionCanceledEvent.Email,
            string.Format(SubjectConstants.SubscriptionCanceled, subscriptionCanceledEvent.Name),
            TemplatePathConstants.SubscriptionCanceled,
            subscriptionCanceledEvent,
            context.CancellationToken
        );
    }
}
