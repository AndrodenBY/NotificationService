using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using SubsTracker.Messaging.Contracts;

namespace NotificationService.Infrastructure.Services;

public class EmailService(
    IFluentEmail email,
    ILogger<EmailService> logger) : IEmailService
{
    public async Task Send<TEvent>(
        string recipientEmail,
        string subject,
        string templatePath,
        TEvent eventModel,
        CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var templateFullPath = Path.Combine(AppContext.BaseDirectory, templatePath);

        var response = await email
            .To(recipientEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templateFullPath, eventModel)
            .SendAsync(cancellationToken);

        HandleResponse(response, recipientEmail);
    }

    private void HandleResponse(SendResponse response, string recipientEmail)
    {
        if (response.Successful)
        {
            logger.LogInformation("Email sent successfully to {Recipient}", recipientEmail);
            return;
        }

        logger.LogError("Email sending failed to {Recipient}. Total errors: {ErrorCount}",
            recipientEmail, response.ErrorMessages.Count);

        foreach (var error in response.ErrorMessages)
        {
            logger.LogError("Email failure reason to {Recipient}: {Error}", recipientEmail, error);
        }
    }
}
