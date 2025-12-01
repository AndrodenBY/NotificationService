using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Repositories;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure.Dependencies;

public static class InfrastructureDependenciesRegister
{
    public static void RegisterNotificationServiceDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEmailServices(configuration);
        services.AddMassTransitConsumers(configuration);

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        services.AddSingleton<IMongoContext>(serviceProvider =>
        {
            var connectionString = configuration["MongoDbConnectionString"];
            var databaseName = configuration["MongoDbNotificationServiceDatabase"];

            return new MongoContext(connectionString!, databaseName!);
        });

        services
            .AddScoped<ILogRepository, MongoLogRepository>()
            .AddScoped<IEmailService, EmailService>()
            .Decorate<IEmailService, EmailServiceLoggingDecorator>();
    }
}
