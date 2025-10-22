using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Consumers;

public class MemberLeftGroupConsumer(IEmailService emailService, ILogger<IEmailService> logger) : IConsumer<MemberLeftGroupEvent>
{
    public async Task Consume(ConsumeContext<MemberLeftGroupEvent> context)
    {
        var memberLeftGroupEvent = context.Message;
        
        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.MemberLeftGroup, memberLeftGroupEvent.Email, memberLeftGroupEvent.GroupName),
            templatePath: TemplatePathConstants.MemberLeftGroup,
            eventModel: memberLeftGroupEvent,
            cancellationToken: context.CancellationToken);
    }
}
