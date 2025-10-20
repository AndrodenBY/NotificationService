using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.MailKitSmtp; 
using NotificationService.Options;
using NotificationService.Interfaces;
using NotificationService.Services;

namespace NotificationService;

public static class DependenciesRegister
{
    public static IServiceCollection RegisterNotificationServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        services.AddFluentEmail(emailSettings["DefaultUserName"], emailSettings["Sender"])
            .AddSmtpSender(emailSettings["SMTPSetting:Host"], emailSettings.GetValue<int>("Port"));
        
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        
        services.AddScoped<IEmailService, EmailService>();
        
        // var host = emailSettings["SMTPSetting:Host"];
        // var port = emailSettings.GetValue<int>("Port");
        // var enableSsl = emailSettings.GetValue<bool>("EnableSsl"); 
        //
        // services.AddFluentEmail(configuration["DefaultSenderEmail"])
        //         .AddRazorRenderer()
        //         .AddMailKitSender(new SmtpClientOptions 
        //         {
        //             Server = host,
        //             Port = port,
        //             User = configuration["DefaultUserName"], 
        //             Password = configuration["SenderEmailPassword"],
        //             UseSsl = enableSsl 
        //         });
        
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(DependenciesRegister).Assembly); 
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                
                cfg.Host(options.HostName, h =>
                {
                    h.Username(configuration["RabbitMq:UserName"]);
                    h.Password(configuration["RabbitMq:Password"]);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
//        services.AddMassTransitHostedService(true);

        return services;
    }
}
