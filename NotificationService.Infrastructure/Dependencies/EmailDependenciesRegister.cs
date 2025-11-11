using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Dependencies;

public static class EmailDependenciesRegister
{
    public static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettingsOptions>(configuration.GetSection(EmailSettingsOptions.SectionName));
        
        var emailSettingsOptions = configuration.GetSection(EmailSettingsOptions.SectionName).Get<EmailSettingsOptions>();

        services.AddFluentEmail(emailSettingsOptions?.DefaultUserName, emailSettingsOptions?.Sender)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettingsOptions?.Host, emailSettingsOptions!.Port);
    }
}
