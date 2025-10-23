using MassTransit;
using NotificationService.Constant;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Consumers;

public class MemberChangedRoleConsumer(IEmailService emailService, ILogger<IEmailService> logger)
    : IConsumer<MemberChangedRoleEvent>
{
    public async Task Consume(ConsumeContext<MemberChangedRoleEvent> context)
    {
        var memberChangedRoleEvent = context.Message;

        await emailService.Send(
            memberChangedRoleEvent.Email,
            string.Format(SubjectConstants.MemberChangedRole, memberChangedRoleEvent.Email,
                memberChangedRoleEvent.GroupName),
            TemplatePathConstants.MemberChangedRole,
            memberChangedRoleEvent,
            context.CancellationToken);
    }
}
