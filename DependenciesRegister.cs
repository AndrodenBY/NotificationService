using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Options;
using NotificationService.Interfaces;
using NotificationService.Services;

namespace NotificationService;

public static class DependenciesRegister
{
    public static IServiceCollection RegisterNotificationServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettingsOptions>(configuration.GetSection(EmailSettingsOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        
        var emailSettingsOptions = configuration.GetSection(EmailSettingsOptions.SectionName).Get<EmailSettingsOptions>();
        
        services.AddFluentEmail(emailSettingsOptions.DefaultUserName, emailSettingsOptions.Sender)
            .AddRazorRenderer()
            .AddSmtpSender(emailSettingsOptions.Host, emailSettingsOptions.Port);
        services.AddScoped<IEmailService, EmailService>();
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(typeof(DependenciesRegister).Assembly); 
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                configurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHost, host =>
                {
                    host.Username(rabbitMqOptions.UserName);
                    host.Password(rabbitMqOptions.Password);
                });
                
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
