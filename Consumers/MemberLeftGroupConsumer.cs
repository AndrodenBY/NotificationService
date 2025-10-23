using MassTransit;
using NotificationService.Constant;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Consumers;

public class MemberLeftGroupConsumer(IEmailService emailService, ILogger<IEmailService> logger)
    : IConsumer<MemberLeftGroupEvent>
{
    public async Task Consume(ConsumeContext<MemberLeftGroupEvent> context)
    {
        var memberLeftGroupEvent = context.Message;

        await emailService.Send(
            memberLeftGroupEvent.Email,
            string.Format(SubjectConstants.MemberLeftGroup, memberLeftGroupEvent.Email, memberLeftGroupEvent.GroupName),
            TemplatePathConstants.MemberLeftGroup,
            memberLeftGroupEvent,
            context.CancellationToken);
    }
}
