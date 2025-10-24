using FluentEmail.Core;
using FluentEmail.Core.Models;
using NotificationService.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Services;

public class EmailService(
    IFluentEmail email,
    ILogger<EmailService> logger) : IEmailService
{
    public async Task Send<TEvent>(string recipientEmail, string subject, string templatePath, TEvent eventModel,
        CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var templateFullPath = Path.Combine(AppContext.BaseDirectory, templatePath);

        var response = await email
            .To(recipientEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templateFullPath, eventModel)
            .SendAsync(cancellationToken);

        ProcessResponse(response, recipientEmail);
    }

    private void ProcessResponse(SendResponse response, string recipientEmail)
    {
        if (!response.Successful)
        {
            logger.LogError(
                "Email sending failed to {Recipient}. Total errors: {ErrorCount}",
                recipientEmail,
                response.ErrorMessages.Count);

            foreach (var error in response.ErrorMessages)
                logger.LogError("Email failure reason to {Recipient}: {Error}", recipientEmail, error);
        }
        else
        {
            logger.LogInformation("Email sent successfully to {Recipient}", recipientEmail);
        }
    }
}
