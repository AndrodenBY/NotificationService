using MassTransit;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Consumers;

public class MemberChangedRoleConsumer(IEmailService emailService, ILogger<IEmailService> logger) : IConsumer<MemberChangedRoleEvent>
{
    public async Task Consume(ConsumeContext<MemberChangedRoleEvent> context)
    {
        var memberChangedRoleEvent = context.Message;
        
        await emailService.Send(
            recipientEmail: EmailConstants.RecepientEmail,
            subject: string.Format(SubjectConstants.MemberChangedRole, memberChangedRoleEvent.Email, memberChangedRoleEvent.GroupName),
            templatePath: TemplatePathConstants.MemberChangedRole,
            eventModel: memberChangedRoleEvent,
            cancellationToken: context.CancellationToken);
    }
}
