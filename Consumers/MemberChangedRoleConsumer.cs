using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Consumers;

public class MemberChangedRoleConsumer(IEmailService emailService) : IConsumer<MemberChangedRoleEvent>
{
    public async Task Consume(ConsumeContext<MemberChangedRoleEvent> context)
    {
        var @event = context.Message;
        
        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.MemberChangedRole, @event.Email, @event.GroupName),
            templatePath: TemplatePathConstants.MemberChangedRole,
            eventModel: @event,
            cancellationToken: context.CancellationToken);
    }
}
