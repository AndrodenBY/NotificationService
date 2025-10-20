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

// Класс EmailService — это тонкая обертка вокруг IFluentEmail
public class EmailService(
    IFluentEmail email, 
    ILogger<EmailService> logger) : IEmailService
{
    public async Task Send<TEvent>(string recipientEmail, string subject, string templatePath, TEvent eventModel, CancellationToken cancellationToken) where TEvent : BaseEvent
    {
        var response = await email
            .To(EmailConstants.RecepientEmail)
            .Subject(subject)
            .UsingTemplateFromFile(templatePath, eventModel) 
            .SendAsync(cancellationToken); 
        
        ProcessResponse(response, EmailConstants.RecepientEmail);
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



// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using FluentEmail.Core;
// using FluentEmail.Core.Models;
// using MailKit.Net.Smtp;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Logging;
// using MimeKit;
// using NotificationService.Interfaces;
//
//
// namespace NotificationService.Services;
//
// public class EmailService(IFluentEmail email, ILogger<EmailService> logger) : IEmailService
// {
//     public async Task Send(string recipientEmail, string subject, string message)
//     {
//         var response = await email
//             .To(recipientEmail)
//             .Subject(subject)
//             .Body(message) 
//             .SendAsync(); 
//     }
//     
//     
//     // public async Task Send(EmailMetadata emailMetadata)
//     // {
//     //     await _fluentEmail.To(emailMetadata.ToAddress)
//     //         .Subject(emailMetadata.Subject)
//     //         .Body(emailMetadata.Body)
//     //         .SendAsync();
//     // }
//     
//     // public async Task Send(T data, string path, string subject, CancellationToken ct)
//     // {
//     //     string templateFile = env.CreatePathToEmailTemplate(path);
//     //     
//     //     var response = await email
//     //         .To(data.Email)
//     //         .Subject(subject)
//     //         .UsingTemplateFromFile(templateFile, data) 
//     //         .SendAsync(ct);
//     // }
//     
// }
