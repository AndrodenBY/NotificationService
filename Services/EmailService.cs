using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using NotificationService.Constant;
using SubsTracker.Messaging.Contracts;
using NotificationService.Interfaces;

namespace NotificationService.Services;

public class EmailService(
    IFluentEmail email, 
    ILogger<EmailService> logger) : IEmailService
{
    public async Task Send<TEvent>(string recipientEmail, string subject, string templatePath, TEvent eventModel, CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var templateFullPath = Path.Combine(AppContext.BaseDirectory, templatePath);
        
        var response = await email
            .To(EmailConstants.RecepientEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templateFullPath, eventModel, isHtml: true) 
            .SendAsync(cancellationToken); 
        
        //ProcessResponse(response, EmailConstants.RecepientEmail);
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
            {
                logger.LogError("Email failure reason to {Recipient}: {Error}", recipientEmail, error);
            }
        }
        else
        {
            logger.LogInformation("Email sent successfully to {Recipient}", recipientEmail);
        }
    }
}
