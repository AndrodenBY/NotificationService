using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;
using NotificationService.Services;

namespace NotificationService.Consumers;

public class SubscriptionCanceledConsumer(IEmailService emailService, ILogger<IEmailService> logger) : IConsumer<SubscriptionCanceledEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionCanceledEvent> context)
    {
        var subscriptionCanceledEvent = context.Message;

        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.SubscriptionCanceled, subscriptionCanceledEvent.Name),
            templatePath: TemplatePathConstants.SubscriptionCanceled,
            eventModel: subscriptionCanceledEvent,
            cancellationToken: context.CancellationToken
            );
    }
}

