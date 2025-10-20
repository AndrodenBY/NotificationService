using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Consumers;

public class MemberLeftGroupConsumer(IEmailService emailService) : IConsumer<MemberLeftGroupEvent>
{
    public async Task Consume(ConsumeContext<MemberLeftGroupEvent> context)
    {
        var @event = context.Message;
        
        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.MemberLeftGroup, @event.Email, @event.GroupName),
            templatePath: TemplatePathConstants.MemberLeftGroup,
            eventModel: @event,
            cancellationToken: context.CancellationToken);
    }
}


