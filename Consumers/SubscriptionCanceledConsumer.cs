using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Consumers;

public class SubscriptionCanceledConsumer(IEmailService emailService) : IConsumer<SubscriptionCanceledEvent>
{
    public async Task Consume(ConsumeContext<SubscriptionCanceledEvent> context)
    {
        var @event = context.Message;

        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.SubscriptionCanceled, @event.Name),
            templatePath: TemplatePathConstants.SubscriptionCanceled,
            eventModel: @event,
            cancellationToken: context.CancellationToken
            );
    }
}


// public class SubscriptionCanceledConsumer(IFluentEmail email) : IConsumer<SubscriptionCanceledEvent>
// {
//     public async Task Consume(ConsumeContext<SubscriptionCanceledEvent> context)
//     {
//         var msg = context.Message;
//         
//         await email
//             .To(EmailConstants.recepientEmail)
//             .Subject($"Subscription cancelled: {msg.SubscriptionName}")
//             .UsingTemplateFromFile("Templates/SubscriptionCanceled.cshtml", msg)
//             .SendAsync(context.CancellationToken);
//     }
// }

