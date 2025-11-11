using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Constants;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Consumers;

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
