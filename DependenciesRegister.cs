using MassTransit;
using Microsoft.Extensions.Options;
using NotificationService.Consumers;
using NotificationService.Interfaces;
using NotificationService.Options;
using NotificationService.Services;

namespace NotificationService;

public static class DependenciesRegister
{
    public static IServiceCollection RegisterNotificationServiceDependencies(this IServiceCollection services,
        IConfiguration configuration)
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
                configurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHostName, host =>
                {
                    host.Username(rabbitMqOptions.UserName);
                    host.Password(rabbitMqOptions.Password);
                });

                configurator.ReceiveEndpoint("subscription-events-queue", endpoint =>
                {
                    endpoint.Durable = true;
                    
                    endpoint.ConfigureConsumer<SubscriptionCanceledConsumer>(context);
                    endpoint.ConfigureConsumer<SubscriptionRenewedConsumer>(context);
                    
                    endpoint.PrefetchCount = 10;
                    endpoint.UseMessageRetry(retry =>
                    {
                        retry.Intervals(100, 500, 1000);
                        retry.Ignore<FormatException>();
                        retry.Ignore<ArgumentException>();
                    });
                });
                
                configurator.ReceiveEndpoint("user-management-queue", endpoint =>
                {
                    endpoint.Durable = true;
                    
                    endpoint.ConfigureConsumer<MemberLeftGroupConsumer>(context);
                    endpoint.ConfigureConsumer<MemberChangedRoleConsumer>(context);
                    
                    endpoint.PrefetchCount = 10;
                    endpoint.UseMessageRetry(retry =>
                    {
                        retry.Intervals(100, 500, 1000);
                        retry.Ignore<FormatException>();
                        retry.Ignore<ArgumentException>();
                    });
                });
            });
        });

        return services;
    }
}
