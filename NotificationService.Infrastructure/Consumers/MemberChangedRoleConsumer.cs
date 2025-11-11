using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Constants;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Consumers;

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
