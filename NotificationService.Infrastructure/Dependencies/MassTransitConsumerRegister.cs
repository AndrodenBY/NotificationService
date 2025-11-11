using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.Consumers;
using NotificationService.Infrastructure.Options;

namespace NotificationService.Infrastructure.Dependencies;

public static class MassTransitConsumerRegister
{
    public static void AddMassTransitConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(typeof(InfrastructureDependenciesRegister).Assembly);
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
                        retry.Ignore<InvalidOperationException>();
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
    }
}
