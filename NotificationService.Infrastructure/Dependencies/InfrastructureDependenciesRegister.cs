using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Options;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure.Dependencies;

public static class InfrastructureDependenciesRegister
{
    public static void RegisterNotificationServiceDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailSettingsOptions>(configuration.GetSection(EmailSettingsOptions.SectionName));
        
        var emailSettingsOptions = configuration.GetSection(EmailSettingsOptions.SectionName).Get<EmailSettingsOptions>();

        services.AddFluentEmail(emailSettingsOptions?.DefaultUserName, emailSettingsOptions.Sender)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettingsOptions.Host, emailSettingsOptions.Port);

        services.AddMassTransitConsumers(configuration);

        services.AddScoped<IEmailService, EmailService>()
                .Decorate<IEmailService, EmailServiceLoggingDecorator>();
    }
}
