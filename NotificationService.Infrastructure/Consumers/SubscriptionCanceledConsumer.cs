using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Constants;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Consumers;

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
