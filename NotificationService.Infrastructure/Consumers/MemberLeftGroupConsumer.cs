using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Constants;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Consumers;

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
